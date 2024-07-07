using System.Collections;
using System.Collections.Generic;
using System.Linq;
using LLVMSharp.Interop;
using Twice.Compiler.Backend.AST;
using Twice.Compiler.Backend.AST.Expressions;
using Twice.Compiler.Backend.AST.Statements;
using Twice.Compiler.Backend.AST.Types;
using TypedAST;

namespace Twice.Compiler.Backend.Codegen;

class StoredFunction(LLVMTypeRef typeRef, LLVMValueRef valueRef)
{
    public LLVMTypeRef fnType = typeRef;
    public LLVMValueRef fnPointer = valueRef;
    public LLVMTypeRef? argStruct;
}

public class CodegenVisitor
{
    private Stack<LLVMValueRef> _valueStack = new();

    private VariableStack<ValueContainer> scope = new();

    private Dictionary<string, StoredFunction> fnScope = new();
    
    private Stack<LLVMBuilderRef> builders = new Stack<LLVMBuilderRef>();
    private LLVMBasicBlockRef currentBlock;
    private Stack<LLVMValueRef> currentFunction = new Stack<LLVMValueRef>();
    
    private LLVMModuleRef _module;

    private LLVMValueRef _main;

    private LLVMValueRef? _asyncRet = null;
    
    private LLVMValueRef _pthreadCreate;
    private LLVMTypeRef _voidFunctionType;
    private LLVMTypeRef _pthreadCreateType;
    private LLVMValueRef _pthreadJoin;
    private LLVMTypeRef _pthreadJoinType;

    private TypeChecker _typeChecker;
    
    public CodegenVisitor(LLVMModuleRef moduleRef, TypeChecker typeChecker)
    {
        _module = moduleRef;

        _typeChecker = typeChecker;

        _main = _module.AddFunction("main", LLVMTypeRef.CreateFunction(LLVMTypeRef.Void, new LLVMTypeRef[] { }));

        _voidFunctionType = LLVMTypeRef.CreateFunction(LLVMTypeRef.CreatePointer(LLVMTypeRef.Void, 0), new LLVMTypeRef[]
        {
            LLVMTypeRef.CreatePointer(LLVMTypeRef.Void, 0)
        });
        
        _pthreadCreateType = LLVMTypeRef.CreateFunction(LLVMTypeRef.Int32, new LLVMTypeRef[]
        {
            LLVMTypeRef.CreatePointer(LLVMTypeRef.Void, 0),
            LLVMTypeRef.CreatePointer(LLVMTypeRef.Void, 0),
            LLVMTypeRef.CreatePointer(_voidFunctionType, 0),
            LLVMTypeRef.CreatePointer(LLVMTypeRef.Void, 0)
        });

        _pthreadJoinType = LLVMTypeRef.CreateFunction(LLVMTypeRef.Int32, new LLVMTypeRef[]
        {
            LLVMTypeRef.CreatePointer(LLVMTypeRef.Void, 0),
            LLVMTypeRef.CreatePointer(LLVMTypeRef.CreatePointer(LLVMTypeRef.Void, 0), 0)
        });
        
        _pthreadCreate = _module.AddFunction("pthread_create", _pthreadCreateType);
        _pthreadJoin = _module.AddFunction("pthread_join", _pthreadJoinType);
        
        currentFunction.Push(_main);
        
        LLVMBasicBlockRef block = _main.AppendBasicBlock("main");
        currentBlock = block;
        LLVMBuilderRef builder = LLVMBuilderRef.Create(_module.Context);
        builder.PositionAtEnd(block);
        
        builders.Push(builder);
    }

    public void Finish()
    {
        builders.Peek().BuildRetVoid();
    }

    void VisitVarExpression(VariableExpression variableExpression)
    {
        LLVMValueRef pointy = scope.Lookup(variableExpression.Variable)!.Value;

        if (_typeChecker.types[variableExpression].Kind == LLVMTypeKind.LLVMStructTypeKind)
        {
            _valueStack.Push(builders.Peek().BuildLoad2(LLVMTypeRef.CreatePointer(LLVMTypeRef.Void, 0), pointy));
            return;
        }
        _valueStack.Push(builders.Peek().BuildLoad2(_typeChecker.types[variableExpression].Kind == LLVMTypeKind.LLVMArrayTypeKind ? LLVMTypeRef.CreatePointer(LLVMTypeRef.Void, 0) : _typeChecker.types[variableExpression], pointy));
    }

    void VisitWhileStatement(WhileStatement whileStatement)
    {
        LLVMBasicBlockRef checkBlock = currentFunction.Peek().AppendBasicBlock("check");
        LLVMBasicBlockRef loopBlock = currentFunction.Peek().AppendBasicBlock("loop");
        LLVMBasicBlockRef afterBlock = currentFunction.Peek().AppendBasicBlock("after");
        
        
        SetupScope();

        LLVMBuilderRef builder = LLVMBuilderRef.Create(_module.Context);
        builder.PositionAtEnd(checkBlock);
        builders.Push(builder);
        currentBlock = checkBlock;
        
        Visit(whileStatement.Expression);
        LLVMValueRef shouldJump = builders.Peek().BuildICmp(LLVMIntPredicate.LLVMIntEQ, _valueStack.Pop(),
            LLVMValueRef.CreateConstInt(LLVMTypeRef.Int1, 1u));

        builders.Peek().BuildCondBr(shouldJump, loopBlock, afterBlock);
        
        builders.Pop();
        CleanupScope();

        SetupScope();

        builder = LLVMBuilderRef.Create(_module.Context);
        builder.PositionAtEnd(loopBlock);
        builders.Push(builder);
        currentBlock = loopBlock;
        
        
        Visit(whileStatement.Statement);
        if (currentBlock.Terminator == null)
            builders.Peek().BuildBr(checkBlock);
        
        builders.Pop();
        CleanupScope();

        builders.Peek().BuildBr(checkBlock);
        
        builders.Peek().PositionAtEnd(afterBlock);
        currentBlock = afterBlock;
    }
    
    void VisitIfStatement(IfStatement ifStatement)
    {
        Visit(ifStatement.Expression);

        LLVMValueRef shouldJump = builders.Peek().BuildICmp(LLVMIntPredicate.LLVMIntEQ, _valueStack.Pop(),
            LLVMValueRef.CreateConstInt(LLVMTypeRef.Int1, 1u));

        LLVMBasicBlockRef thenBlock = currentFunction.Peek().AppendBasicBlock("thenBlock");
        LLVMBasicBlockRef? elseBlock = null;
        if (ifStatement.ElseStatement != null)
            elseBlock = currentFunction.Peek().AppendBasicBlock("elseBlock");
        LLVMBasicBlockRef afterBlock = currentFunction.Peek().AppendBasicBlock("afterBlock");
        
        SetupScope();

        LLVMBuilderRef builder = LLVMBuilderRef.Create(_module.Context);
        builder.PositionAtEnd(thenBlock);
        builders.Push(builder);
        currentBlock = thenBlock;
        
        Visit(ifStatement.Statement);
        if (currentBlock.Terminator == null)
            builders.Peek().BuildBr(afterBlock);
        builders.Pop();
        
        CleanupScope();

        if (elseBlock != null)
        {
            SetupScope();
            builder = LLVMBuilderRef.Create(_module.Context);
            builder.PositionAtEnd((LLVMBasicBlockRef)elseBlock);
            builders.Push(builder);
            currentBlock = (LLVMBasicBlockRef)elseBlock;
            Visit(ifStatement.ElseStatement!);
            if (currentBlock.Terminator == null)
                builders.Peek().BuildBr(afterBlock);
            builders.Pop();
        
            CleanupScope();
        }
        
        builders.Peek().BuildCondBr(shouldJump, thenBlock, elseBlock ?? afterBlock);

        currentBlock = afterBlock;
        
        builders.Peek().PositionAtEnd(afterBlock);
    }
    
    void VisitAssignmentStatement(AssignmentStatement assignmentStatement)
    {
        Visit(assignmentStatement.Value);

        builders.Peek().BuildStore(_valueStack.Pop(), scope.Lookup(assignmentStatement.Name)!.Value);
    }
    
    void VisitIntExpression(IntExpression e)
    {
        _valueStack.Push(LLVMValueRef.CreateConstInt(LLVMTypeRef.Int32, (ulong)e.Value));
    }

    void VisitStringExpression(StringExpression str)
    {
        string c = str.Value + "\0";

        LLVMValueRef strMemory = builders.Peek().BuildMalloc(LLVMTypeRef.CreateArray(LLVMTypeRef.Int8, (uint)c.Length));

        builders.Peek().BuildStore(LLVMValueRef.CreateConstArray(LLVMTypeRef.Int8, 
                                               c.Select(x => LLVMValueRef.CreateConstInt(LLVMTypeRef.Int8, x))
                                                   .ToArray()), strMemory);
        _valueStack.Push(strMemory);
    }

    void VisitArrayAccessExpression(ArrayAccessExpression e)
    {
        Visit(e.Array);
        Visit(e.Index);
        LLVMValueRef idx = _valueStack.Pop();
        LLVMValueRef arrPointy = _valueStack.Pop();

        LLVMValueRef varPointy = builders.Peek().BuildGEP2(_typeChecker.types[e.Array], arrPointy,
            new LLVMValueRef[] { LLVMValueRef.CreateConstInt(LLVMTypeRef.Int32, 0), idx });
        
        _valueStack.Push(builders.Peek().BuildLoad2(_typeChecker.types[e], varPointy));
    }

    void VisitBinOpExpression(BinOpExpression e)
    {
        Visit(e.Right);
        Visit(e.Left);

        LLVMValueRef lhs = _valueStack.Pop();
        LLVMValueRef rhs = _valueStack.Pop();
        
        
        switch (e.Operation)
        {
            case BinaryOperation.Equality:
                if (lhs.TypeOf.Kind == LLVMTypeKind.LLVMDoubleTypeKind)
                {
                    _valueStack.Push(builders.Peek().BuildFCmp(LLVMRealPredicate.LLVMRealOEQ, lhs, rhs));
                    return;
                }
                _valueStack.Push(builders.Peek().BuildICmp(LLVMIntPredicate.LLVMIntEQ, lhs, rhs));
                return;
            case BinaryOperation.Inequality:
                if (lhs.TypeOf.Kind == LLVMTypeKind.LLVMDoubleTypeKind)
                {
                    _valueStack.Push(builders.Peek().BuildFCmp(LLVMRealPredicate.LLVMRealONE, lhs, rhs));
                    return;
                }
                _valueStack.Push(builders.Peek().BuildICmp(LLVMIntPredicate.LLVMIntNE, lhs, rhs));
                return;
            
            case BinaryOperation.GT:
                if (lhs.TypeOf.Kind == LLVMTypeKind.LLVMDoubleTypeKind)
                {
                    _valueStack.Push(builders.Peek().BuildFCmp(LLVMRealPredicate.LLVMRealOGT, lhs, rhs));
                    return;
                }
                _valueStack.Push(builders.Peek().BuildICmp(LLVMIntPredicate.LLVMIntSGT, lhs, rhs));
                return;
            
            case BinaryOperation.GTE:
                if (lhs.TypeOf.Kind == LLVMTypeKind.LLVMDoubleTypeKind)
                {
                    _valueStack.Push(builders.Peek().BuildFCmp(LLVMRealPredicate.LLVMRealOGE, lhs, rhs));
                    return;
                }
                _valueStack.Push(builders.Peek().BuildICmp(LLVMIntPredicate.LLVMIntSGE, lhs, rhs));
                return;
            case BinaryOperation.LT:
                if (lhs.TypeOf.Kind == LLVMTypeKind.LLVMDoubleTypeKind)
                {
                    _valueStack.Push(builders.Peek().BuildFCmp(LLVMRealPredicate.LLVMRealOLT, lhs, rhs));
                    return;
                }
                _valueStack.Push(builders.Peek().BuildICmp(LLVMIntPredicate.LLVMIntSLT, lhs, rhs));
                return;
            case BinaryOperation.LTE:
                if (lhs.TypeOf.Kind == LLVMTypeKind.LLVMDoubleTypeKind)
                {
                    _valueStack.Push(builders.Peek().BuildFCmp(LLVMRealPredicate.LLVMRealOLE, lhs, rhs));
                    return;
                }
                _valueStack.Push(builders.Peek().BuildICmp(LLVMIntPredicate.LLVMIntSLE, lhs, rhs));
                return;
            case BinaryOperation.Addition:
                if (lhs.TypeOf.Kind == LLVMTypeKind.LLVMDoubleTypeKind)
                {
                    _valueStack.Push(builders.Peek().BuildFAdd(lhs, rhs));
                    return;
                }
                _valueStack.Push(builders.Peek().BuildAdd(lhs, rhs));
                return;
            case BinaryOperation.Subtraction:
                if (lhs.TypeOf.Kind == LLVMTypeKind.LLVMDoubleTypeKind)
                {
                    _valueStack.Push(builders.Peek().BuildFSub(lhs, rhs));
                    return;
                }
                _valueStack.Push(builders.Peek().BuildSub(lhs, rhs));
                return;
            case BinaryOperation.Multiplication:
                if (lhs.TypeOf.Kind == LLVMTypeKind.LLVMDoubleTypeKind)
                {
                    _valueStack.Push(builders.Peek().BuildFMul(lhs, rhs));
                    return;
                }
                _valueStack.Push(builders.Peek().BuildMul(lhs, rhs));
                return;
            case BinaryOperation.Division:
                if (lhs.TypeOf.Kind == LLVMTypeKind.LLVMDoubleTypeKind)
                {
                    _valueStack.Push(builders.Peek().BuildFDiv(lhs, rhs));
                    return;
                }
                _valueStack.Push(builders.Peek().BuildSDiv(lhs, rhs));
                return;
            case BinaryOperation.Remainder:
                _valueStack.Push(builders.Peek().BuildSRem(lhs, rhs));
                return;
            
            case BinaryOperation.LogicalAnd:
                _valueStack.Push(builders.Peek().BuildAnd(lhs, rhs));
                return;
            
            case BinaryOperation.LogicalOr:
                _valueStack.Push(builders.Peek().BuildOr(lhs, rhs));
                return;
        }
    }

    void VisitNegateExpression(NegateExpression negate)
    {
        Visit(negate.Expression);
        
        _valueStack.Push(
            builders.Peek().BuildXor(
                _valueStack.Pop(), 
                LLVMValueRef.CreateConstInt(LLVMTypeRef.Int1, 1u)
                )
            );
    }
    
    void VisitBoolExpression(BoolExpression boolean)
    {
        _valueStack.Push(LLVMValueRef.CreateConstInt(LLVMTypeRef.Int1, boolean.Value ? 1u : 0u));
    }

    void VisitFloatExpression(FloatExpression floaty)
    {
        _valueStack.Push(LLVMValueRef.CreateConstReal(LLVMTypeRef.Double, floaty.Value));
    }

    void VisitArrayLiteralExpresssion(ArrayLiteralExpression arrayLiteral)
    {
        arrayLiteral.Items.ToArray().Reverse().ToList().ForEach(Visit);

        LLVMValueRef[] vals = new LLVMValueRef[arrayLiteral.Items.Count];

        for (int i = 0; i < arrayLiteral.Items.Count; i++)
        {
            vals[i] = _valueStack.Pop();
        }

        LLVMValueRef pointy = builders.Peek().BuildMalloc(_typeChecker.types[arrayLiteral]);

        builders.Peek().BuildStore(LLVMValueRef.CreateConstArray(_typeChecker.types[arrayLiteral].ElementType, vals), pointy);
        
        _valueStack.Push(pointy);

    }

    void VisitExpression(IExpression e)
    {
        if (e is IntExpression inte)
        {
            VisitIntExpression(inte);
            return;
        }

        if (e is BinOpExpression binope)
        {
            VisitBinOpExpression(binope);
            return;
        }

        if (e is AwaitExpression awt)
        {
            VisitAwaitExpression(awt);
            return;
        }

        if (e is NegateExpression negate)
        {
            VisitNegateExpression(negate);
            return;
        }

        if (e is ArrayAccessExpression arracc)
        {
            VisitArrayAccessExpression(arracc);
            return;
        }
        
        if (e is StringExpression str)
        {
            VisitStringExpression(str);
            return;
        }

        if (e is VariableExpression variableExpression)
        {
            VisitVarExpression(variableExpression);
        }

        if (e is BlockExpression blockExpression)
        {
            VisitBlockExpression(blockExpression);
        }

        if (e is BoolExpression boolean)
        {
            VisitBoolExpression(boolean);
        }

        if (e is FloatExpression floaty)
        {
            VisitFloatExpression(floaty);
        }

        if (e is ArrayLiteralExpression arrayLiteral)
        {
            VisitArrayLiteralExpresssion(arrayLiteral);
        }
        
        if (e is FunctionCallExpression fc)
        {
            VisitFunctionCallExpression(fc);
            return;
        }
    }

    void VisitExternalStatement(ExternFunctionStatement externFunctionStatement)
    {
        var fnType = _typeChecker.types[externFunctionStatement];
        var fnPointer = _module.AddFunction(externFunctionStatement.FunctionName, fnType);

        fnScope[externFunctionStatement.FunctionName] = new StoredFunction(fnType, fnPointer);
    }

    void VisitBlockStatement(BlockStatement block)
    {
        var tempScope = new VariableStack<ValueContainer>(scope);
        scope = tempScope;

        block.Statements.ForEach(Visit);
        
        scope = scope.GetUpper()!;
    }
   
    void VisitBlockExpression(BlockExpression block)
    {
        var tempScope = new VariableStack<ValueContainer>(scope);
        scope = tempScope;

        block.Statements.ForEach(Visit);

        Visit(block.ReturnExpression);
        
        scope = scope.GetUpper()!;
    }
    
    void VisitReturnStatement(ReturnStatement ret)
    {
        if (ret.Value != null)
            Visit(ret.Value);

        if (_asyncRet is null)
        {
            if (ret.Value != null)
            {
                builders.Peek().BuildRet(_valueStack.Pop());
            }
            else
            {
                builders.Peek().BuildRetVoid();
            }
        }
        else
        {
            if (ret.Value != null)
                builders.Peek().BuildStore(_valueStack.Pop(), (LLVMValueRef)_asyncRet);
            builders.Peek().BuildRet((LLVMValueRef)_asyncRet);
        }
    }

    void VisitAwaitStatement(AwaitStatement awt)
    {
        Visit(awt.Inner);

        LLVMValueRef threadid = _valueStack.Pop();
        
        LLVMValueRef lptr = builders.Peek().BuildLoad2(LLVMTypeRef.CreatePointer(LLVMTypeRef.Void, 0), threadid);

        builders.Peek().BuildCall2(_pthreadJoinType, _pthreadJoin, new LLVMValueRef[]
        {
            lptr,
            LLVMValueRef.CreateConstNull(LLVMTypeRef.CreatePointer(LLVMTypeRef.Void, 0))
        });
        
    }

    void VisitAwaitExpression(AwaitExpression awt)
    {
        Visit(awt.Inner);
        
        LLVMValueRef threadid = _valueStack.Pop();
        
        LLVMValueRef retSpot = builders.Peek().BuildMalloc(LLVMTypeRef.CreatePointer(_typeChecker.types[awt], 0));
        
        LLVMValueRef lptr = builders.Peek().BuildLoad2(LLVMTypeRef.CreatePointer(LLVMTypeRef.Void, 0), threadid);

        builders.Peek().BuildCall2(_pthreadJoinType, _pthreadJoin, new LLVMValueRef[]
        {
            lptr,
            retSpot
        });

        _valueStack.Push(builders.Peek().BuildLoad2(_typeChecker.types[awt],
            builders.Peek().BuildLoad2(LLVMTypeRef.CreatePointer(LLVMTypeRef.Void, 0), retSpot)));
    }
    
    void VisitFunctionCallStatement(FunctionCallStatement functionCallStatement)
    {
        // visit function arguments in reverse order
        for (int i = functionCallStatement.Arguments.Count - 1; i >= 0; i--)
        {
            Visit(functionCallStatement.Arguments[i]);
        }

        LLVMValueRef[] args = new LLVMValueRef[functionCallStatement.Arguments.Count];

        if (fnScope[functionCallStatement.FunctionName].argStruct != null)
        {
            LLVMValueRef myStruct = builders.Peek().BuildMalloc((LLVMTypeRef)fnScope[functionCallStatement.FunctionName].argStruct);

            for (uint i = 0; i < args.Length; i++)
            {
                builders.Peek().BuildStore(_valueStack.Pop(), builders.Peek().BuildStructGEP2((LLVMTypeRef)fnScope[functionCallStatement.FunctionName].argStruct, myStruct, i));
            }
            
            LLVMValueRef threadid = builders.Peek().BuildMalloc(LLVMTypeRef.CreatePointer(LLVMTypeRef.Void, 0));

            builders.Peek().BuildCall2(_pthreadCreateType, _pthreadCreate, new LLVMValueRef[]
            {
                threadid,
                LLVMValueRef.CreateConstNull(LLVMTypeRef.CreatePointer(LLVMTypeRef.Void, 0)),
                fnScope[functionCallStatement.FunctionName].fnPointer,
                myStruct
            });
        }
        else
        {
            builders.Peek().BuildCall2(fnScope[functionCallStatement.FunctionName].fnType,
                fnScope[functionCallStatement.FunctionName].fnPointer,
                args.Select(_ => _valueStack.Pop()).ToArray());
        }
        
    }
    
    
    void VisitFunctionCallExpression(FunctionCallExpression functionCallExpression)
    { 
        // visit function arguments in reverse order
        for (int i = functionCallExpression.Arguments.Count - 1; i >= 0; i--)
        {
            Visit(functionCallExpression.Arguments[i]);
        }

        LLVMValueRef[] args = new LLVMValueRef[functionCallExpression.Arguments.Count];

        if (fnScope[functionCallExpression.FunctionName].argStruct != null)
        {
            LLVMValueRef myStruct = builders.Peek().BuildMalloc((LLVMTypeRef)fnScope[functionCallExpression.FunctionName].argStruct);

            for (uint i = 0; i < args.Length; i++)
            {
                builders.Peek().BuildStore(_valueStack.Pop(), builders.Peek().BuildStructGEP2((LLVMTypeRef)fnScope[functionCallExpression.FunctionName].argStruct, myStruct, i));
            }
            
            LLVMValueRef threadid = builders.Peek().BuildMalloc(LLVMTypeRef.CreatePointer(LLVMTypeRef.Void, 0));

            builders.Peek().BuildCall2(_pthreadCreateType, _pthreadCreate, new LLVMValueRef[]
            {
                threadid,
                LLVMValueRef.CreateConstNull(LLVMTypeRef.CreatePointer(LLVMTypeRef.Void, 0)),
                fnScope[functionCallExpression.FunctionName].fnPointer,
                myStruct
            });
            
            _valueStack.Push(threadid);
        }
        else
        {
            _valueStack.Push(builders.Peek().BuildCall2(fnScope[functionCallExpression.FunctionName].fnType,
                fnScope[functionCallExpression.FunctionName].fnPointer,
                args.Select(_ => _valueStack.Pop()).ToArray()));
        }
    }

    void VisitDeclarationStatement(DeclarationStatement declaration)
    {
        LLVMTypeRef type;

        //LLVMValueRef global = _module.AddGlobal(type, declaration.Name);

        Visit(declaration.Value);
        LLVMValueRef val = _valueStack.Pop();
        LLVMValueRef variable = builders.Peek().BuildMalloc(val.TypeOf, declaration.Name);

        builders.Peek().BuildStore(val, variable);
        
        scope.SetValue(declaration.Name, new ValueContainer(variable));

    }

    void SetupScope()
    {
        var tempStack = new VariableStack<ValueContainer>(scope);
        scope = tempStack;
    }

    void CleanupScope()
    {
        scope = scope.GetUpper()!;
    }

    void VisitDefinitionStatement(FunctionDefinitionStatement def)
    {
        var tempStack = new VariableStack<ValueContainer>(scope);
        scope = tempStack;

        var funcType = !def.Async
            ? LLVMTypeRef.CreateFunction(_typeChecker.types[def],
                def.Arguments.Select(x => _typeChecker.types[x]).ToArray())
            : _voidFunctionType;

        var func = _module.AddFunction(def.FunctionName, funcType);
        
        fnScope[def.FunctionName] = new StoredFunction(funcType, func);

        var block = func.AppendBasicBlock("entry");
        var builder = LLVMBuilderRef.Create(_module.Context);
        builder.PositionAtEnd(block);
        builders.Push(builder);
        currentBlock = block;
        
        currentFunction.Push(func);
        if (def.Async)
            _asyncRet = builders.Peek().BuildMalloc(_typeChecker.types[def] == LLVMTypeRef.Void ? LLVMTypeRef.Int32 : _typeChecker.types[def]);

        if (def.Async)
        {

            LLVMTypeRef structy = LLVMTypeRef.CreateStruct((LLVMTypeRef[])def.Arguments.Select(x => _typeChecker.types[x]).ToArray(), false);

            fnScope[def.FunctionName].argStruct = structy;
            
            for (int i = 0; i < def.Arguments.Count; i++)
            {
                LLVMValueRef pointy = builders.Peek().BuildMalloc(_typeChecker.types[def.Arguments[i]]);
                builders.Peek().BuildStore(builders.Peek().BuildLoad2(_typeChecker.types[def.Arguments[i]], builders.Peek().BuildStructGEP2(structy, func.Params[0], (uint)i)), pointy);
                scope.SetValue(def.Arguments[i].Name, new ValueContainer(pointy));
            }
        }
        else
        {
            for (int i = 0; i < func.ParamsCount; i++)
            {
                LLVMValueRef pointy = builders.Peek().BuildMalloc(funcType.ParamTypes[i]);
                builders.Peek().BuildStore(func.Params[i], pointy);
                scope.SetValue(def.Arguments[i].Name, new ValueContainer(pointy));
            }
        }
        
        Visit(def.InnerNode);

        if (_typeChecker.types[def.InnerNode] == LLVMTypeRef.Void)
        {
            if (def.Async)
            {
                builders.Peek().BuildRet((LLVMValueRef)_asyncRet);
            }
            else
            {
                builders.Peek().BuildRetVoid();
            }
        }
        
        if (def.InnerNode is IExpression)
        {
            if (def.Async)
            {
                builders.Peek().BuildStore(_valueStack.Pop(), (LLVMValueRef)_asyncRet);
                builders.Peek().BuildRet((LLVMValueRef)_asyncRet);
            }
            else
            {
                builders.Peek().BuildRet(_valueStack.Pop());
            }
        }


        _asyncRet = null;
        currentFunction.Pop();
        builders.Pop();
        scope = scope.GetUpper()!;
    }
    
    void VisitStatement(IStatement s)
    {
        if (s is DeclarationStatement decl)
        {
            VisitDeclarationStatement(decl);
            return;
        }

        if (s is BlockStatement block)
        {
            VisitBlockStatement(block);
            return;
        }

        if (s is FunctionCallStatement fc)
        {
            VisitFunctionCallStatement(fc);
            return;
        }

        if (s is AwaitStatement awt)
        {
            VisitAwaitStatement(awt);
            return;
        }
        
        if (s is AssignmentStatement assign)
        {
            VisitAssignmentStatement(assign);
            return;
        }

        if (s is ExternFunctionStatement ext)
        {
            VisitExternalStatement(ext);
            return;
        }

        if (s is ReturnStatement ret)
        {
            VisitReturnStatement(ret);
            return;
        }

        if (s is IfStatement ifs)
        {
            VisitIfStatement(ifs);
            return;
        }

        if (s is WhileStatement whileStatement)
        {
            VisitWhileStatement(whileStatement);
            return;
        }

        if (s is FunctionDefinitionStatement def)
        {
            VisitDefinitionStatement(def);
            return;
        }
    }
    
    public void Visit(ASTNode node)
    {
        if (node is IStatement statement)
        {
            VisitStatement(statement);
            return;
        }

        if (node is IExpression expression)
        {
            VisitExpression(expression);
            return;
        }
    }
    
}