using System;
using System.Collections.Generic;
using System.Linq;
using LLVMSharp.Interop;
using Twice.Compiler.Backend.AST;
using Twice.Compiler.Backend.AST.Expressions;
using Twice.Compiler.Backend.AST.Statements;
using Twice.Compiler.Backend.AST.Types;

namespace TypedAST;

public class TypeContainer
{
    public LLVMTypeRef Type;

    public TypeContainer(LLVMTypeRef typeRef)
    {
        Type = typeRef;
    }
}


public class ValueContainer
{
    public LLVMValueRef Value;

    public ValueContainer(LLVMValueRef refer)
    {
        Value = refer;
    }
}

public class TypeChecker
{
    public Dictionary<ASTNode, LLVMTypeRef> types;

    private VariableStack<TypeContainer> typeStack;

    public Dictionary<ASTNode, LLVMTypeRef> realReturn;
    
    public TypeChecker(TwiceChunk root)
    {
        types = new Dictionary<ASTNode, LLVMTypeRef>();
        typeStack = new VariableStack<TypeContainer>();

        root.Statements.ForEach(Visit);
    }

    void VisitStatement(IStatement statement)
    {
        switch (statement)
        {
            case BlockStatement block:
            {
                var newStack = new VariableStack<TypeContainer>(typeStack);
                typeStack = newStack;
                block.Statements.ForEach(Visit);

                var rets = block.Statements.FindAll(x => types.ContainsKey(x)).Select(x => types[x]).ToList();

                if (rets.Count > 0)
                {
                    bool notSame = rets.Any(x => x != rets[0]);
                    if (notSame)
                        throw new TypeException($"block return types are not all same");

                    types[block] = rets[0];
                }
                else
                {
                    types[block] = LLVMTypeRef.Void;
                }
                
                typeStack = typeStack.GetUpper() ?? throw new InvalidOperationException();
                break;
            }
            case AssignmentStatement assign: 
            {
                if (!typeStack.IsDefined(assign.Name))
                    throw new TypeException($"could not find variable {assign.Name}");

                LLVMTypeRef lkup = typeStack.Lookup(assign.Name)!.Type;

                if (lkup.Kind == LLVMTypeKind.LLVMFunctionTypeKind)
                    throw new TypeException($"cannot use function as variable");
                
                Visit(assign.Value);

                if (lkup != types[assign.Value])
                    throw new TypeException($"type of variable and type of expression don't match in assignment of {assign.Name}");
                break;
            }

            case IfStatement ifs:
            {
                Visit(ifs.Expression);
                if (types[ifs.Expression] != LLVMTypeRef.Int1)
                    throw new TypeException("type of condition in if statement must be boolean");

                Visit(ifs.Statement);

                LLVMTypeRef typeThen = types.TryGetValue(ifs.Statement, out var type) ? type : LLVMTypeRef.Void;
                if (ifs.ElseStatement != null)
                {
                    Visit(ifs.ElseStatement);
                    if ((types.TryGetValue(ifs.ElseStatement, out var elseType) ? elseType : LLVMTypeRef.Void) !=
                        typeThen)
                        throw new TypeException("not all paths in if statement return same type");
                }
                else
                {
                    //typeThen = LLVMTypeRef.Void;
                }

                types[ifs] = typeThen;

                break;
            }
            
            case ReturnStatement ret:
            {
                if (ret.Value != null)
                    Visit(ret.Value);

                types[ret] = ret.Value != null ? types[ret.Value] : LLVMTypeRef.Void;
                break;
            }
            case DeclarationStatement decl:
            {
                Visit(decl.Value);

                if (!typeStack.SetValue(decl.Name, new TypeContainer(types[decl.Value])))
                    throw new TypeException($"variable already defined");

                break;
            }
            
            case AwaitStatement awt:
            {
                Visit(awt.Inner);
                
                if (types[awt.Inner].Kind != LLVMTypeKind.LLVMStructTypeKind)
                    throw new TypeException("tried to await on non-promise value");
                
                break;
            }
            
            case ExternFunctionStatement ext:
            {
                if (typeStack.GetUpper() != null)
                    throw new TypeException("function declaration only allowed in top-level");
                if (ext.FunctionType.GenericName() != "function")
                    throw new TypeException($"type in extern has to be function");

                types[ext] = ToLLVM(ext.FunctionType, ext.Vararg);
                typeStack.SetValue(ext.FunctionName, new TypeContainer(types[ext]));
                break;
            }

            case FunctionCallStatement call:
            {
                call.Arguments.ForEach(Visit);
                break;
            }

            case WhileStatement whileStatement:
            {
                Visit(whileStatement.Expression);

                if (types[whileStatement.Expression] != LLVMTypeRef.Int1)
                    throw new TypeException("expression in while not boolean");
                
                Visit(whileStatement.Statement);

                types[whileStatement] = types.ContainsKey(whileStatement.Statement) ? types[whileStatement.Statement] : LLVMTypeRef.Void;
                break;
            }

            case FunctionDefinitionStatement def:
            {

                if (def.FunctionName == "main")
                    throw new TypeException("function may not be called main");
                
                if (typeStack.GetUpper() != null)
                    throw new TypeException("function declaration only allowed in top-level");

                if (def.Async && def.ReturnType.GenericName() != "promise")
                    throw new TypeException("async functions must return a promise type");

                var oldStack = typeStack;
                typeStack = new VariableStack<TypeContainer>();

                def.Arguments.ForEach(Visit);

                var fnType = LLVMTypeRef.CreateFunction(ToLLVM(def.ReturnType),
                    def.Arguments.Select(x => types[x]).ToArray());
                
                def.Arguments.ForEach(c =>
                {
                    typeStack.SetValue(c.Name, new TypeContainer(ToLLVM(c.Type)));
                });

                typeStack.SetValue(def.FunctionName, new TypeContainer(fnType));
                
                Visit(def.InnerNode);
                
                if ((def.Async ? LLVMTypeRef.CreateStruct(new LLVMTypeRef[] {types[def.InnerNode]}, false) : types[def.InnerNode]) != ToLLVM(def.ReturnType))
                    throw new TypeException("function definition does not match signature return type");

                types[def] = types[def.InnerNode];
                
                typeStack = oldStack;
                typeStack.SetValue(def.FunctionName, new TypeContainer(fnType));
                break;
            }
        }
    }

    public LLVMTypeRef ToLLVM(IType type, bool fnVararg = false)
    {
        switch (type)
        {
            case ArrayType arr:
                return LLVMTypeRef.CreatePointer(ToLLVM(arr.InnerType), 0);
            case BoolType:
                return LLVMTypeRef.Int1;
            case FloatType:
                return LLVMTypeRef.Double;
            case IntType:
                return LLVMTypeRef.Int32;
            case StringType:
                return LLVMTypeRef.CreatePointer(LLVMTypeRef.Int8, 0);
            case VoidType:
                return LLVMTypeRef.Void;
            case PromiseType pr:
                return LLVMTypeRef.CreateStruct(new LLVMTypeRef[] {ToLLVM(pr.InnerType)}, false);
            case FunctionType fn:
                return LLVMTypeRef.CreateFunction(ToLLVM(fn.ReturnType),
                    fn.ArgumentTypes.Select(x => ToLLVM(x)).ToArray(), fnVararg);
        }

        return LLVMTypeRef.Void;
    }
    
    void VisitExpression(IExpression expression)
    {
        switch (expression)
        {
            case StringExpression:
                types[expression] = LLVMTypeRef.CreatePointer(LLVMTypeRef.Int8, 0);
                return;
            case ArrayAccessExpression arrayAccess:
            {
                Visit(arrayAccess.Array);
                Visit(arrayAccess.Index);

                if (types[arrayAccess.Index].Kind != LLVMTypeKind.LLVMIntegerTypeKind)
                    throw new TypeException("array access index not a number");

                if (types[arrayAccess.Array].Kind != LLVMTypeKind.LLVMArrayTypeKind)
                    throw new TypeException("left side of array access not an array");

                types[expression] = types[arrayAccess.Array].ElementType;
                break;
            }

            case NegateExpression neg:
            {
                Visit(neg.Expression);

                if (types[neg.Expression] != LLVMTypeRef.Int1)
                    throw new TypeException("negation only allowed on booleans");
                
                types[neg] = LLVMTypeRef.Int1;
                break;
            }
            
            case AwaitExpression awt:
            {
                Visit(awt.Inner);
               
                if (types[awt.Inner].Kind != LLVMTypeKind.LLVMStructTypeKind)
                    throw new TypeException("tried to await on non-promise value");
                
                types[awt] = types[awt.Inner].StructElementTypes[0];
                
                break;
            }
            
            case ArrayLiteralExpression { Items.Count: 0 }:
                throw new TypeException("empty array literal");
            case ArrayLiteralExpression arrayLiteral:
            {
                arrayLiteral.Items.ForEach(Visit);

                LLVMTypeRef firstType = types[arrayLiteral.Items[0]];

                if (arrayLiteral.Items.Any(x => types[x] != firstType))
                    throw new TypeException("types in array don't match");

                types[expression] = LLVMTypeRef.CreateArray(firstType, (uint)arrayLiteral.Items.Count);
                break;
            }
            case BinOpExpression binOp:
            {
                Visit(binOp.Left);
                Visit(binOp.Right);

                if (types[binOp.Left] != types[binOp.Right])
                    throw new TypeException($"left and right side of {binOp.Operation.ToString()} do not match");

                switch (binOp.Operation)
                {
                    case BinaryOperation.Addition:
                    case BinaryOperation.Division:
                    case BinaryOperation.Multiplication:
                    case BinaryOperation.Subtraction:        
                        if (types[binOp.Left].Kind != LLVMTypeKind.LLVMIntegerTypeKind && 
                            types[binOp.Left].Kind != LLVMTypeKind.LLVMDoubleTypeKind)
                            throw new TypeException("attempt to perform arithmetic with non-numeric value");
                        types[expression] = types[binOp.Left];
                        break;
                   
                    case BinaryOperation.Remainder:
                        if (types[binOp.Left] != LLVMTypeRef.Int32)
                            throw new TypeException("modulo requires int types");
                        types[expression] = types[binOp.Left];
                        break;
                    
                    case BinaryOperation.LTE:
                    case BinaryOperation.LT:
                    case BinaryOperation.GTE:
                    case BinaryOperation.GT:        
                        if (types[binOp.Left].Kind != LLVMTypeKind.LLVMIntegerTypeKind && 
                            types[binOp.Left].Kind != LLVMTypeKind.LLVMDoubleTypeKind)
                            throw new TypeException("attempt to perform arithmetic with non-numeric value");
                        types[expression] = LLVMTypeRef.Int1;
                        break;
                    
                    case BinaryOperation.Equality:
                    case BinaryOperation.Inequality:
                        if (types[binOp.Left] != types[binOp.Right])
                            throw new TypeException("left and right of equality check don't match");

                        List<LLVMTypeKind> allowedKinds = new List<LLVMTypeKind>() { LLVMTypeKind.LLVMIntegerTypeKind, LLVMTypeKind.LLVMDoubleTypeKind };

                        if (!allowedKinds.Contains(types[binOp.Left].Kind))
                            throw new TypeException("invalid type in equality check");

                        types[expression] = LLVMTypeRef.Int1;
                        break;
                    case BinaryOperation.LogicalAnd:
                    case BinaryOperation.LogicalOr:
                        if (types[binOp.Left] != types[binOp.Right])
                            throw new TypeException("left and right of logical operation don't match");

                        if (types[binOp.Left] != LLVMTypeRef.Int1)
                            throw new TypeException("logical operations are only allowed on bools");

                        types[expression] = types[binOp.Left];
                        break;
                }

                break;
            }
            case BlockExpression block:
            {
                var newStack = new VariableStack<TypeContainer>(typeStack);
                typeStack = newStack;
            
                block.Statements.ForEach(Visit);
            
                Visit(block.ReturnExpression);

                types[expression] = types[block.ReturnExpression];

                typeStack = typeStack.GetUpper() ?? throw new InvalidOperationException();
                break;
            }
            case BoolExpression:
            {
                types[expression] = LLVMTypeRef.Int1;
                break;
            }
            case FloatExpression:
            {
                types[expression] = LLVMTypeRef.Double;
                break;
            }
            case FunctionCallExpression functionCall:
            {
                if (!typeStack.IsDefined(functionCall.FunctionName))
                    throw new TypeException("nonexistant function");

                LLVMTypeRef fnc = typeStack.Lookup(functionCall.FunctionName)!.Type;

                if (fnc.Kind != LLVMTypeKind.LLVMFunctionTypeKind)
                    throw new TypeException("attempt to call non-function");

                if (fnc.IsFunctionVarArg)
                {
                    types[expression] = fnc.ReturnType;
                    break;
                }

                functionCall.Arguments.ForEach(Visit);
                
                if (fnc.ParamTypesCount != functionCall.Arguments.Count)
                    throw new TypeException("function argument count doesn't match");

                for (int i = 0; i < fnc.ParamTypesCount; i++)
                {
                    if (fnc.ParamTypes[i] != types[functionCall.Arguments[i]])
                        throw new TypeException(
                            $"function argument {i + 1} on {functionCall.FunctionName} call does not match type signature");
                }

                types[expression] = fnc.ReturnType;
                break;
            }
            case IntExpression:
            {
                types[expression] = LLVMTypeRef.Int32;
                break;
            }
            case VariableExpression variableExpression:
            {
                if (!typeStack.IsDefined(variableExpression.Variable))
                    throw new TypeException($"could not find variable {variableExpression.Variable}");

                LLVMTypeRef lkup = typeStack.Lookup(variableExpression.Variable)!.Type;

                if (lkup.Kind == LLVMTypeKind.LLVMFunctionTypeKind)
                    throw new TypeException($"cannot use function as variable");

                types[expression] = lkup;
                break;
            }
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

        if (node is FunctionArgument arg)
        {
            types[arg] = ToLLVM(arg.Type);
            return;
        }
    }
    
}