using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Twice.Compiler.Backend.AST;
using Twice.Compiler.Backend.AST.Expressions;
using Twice.Compiler.Backend.AST.Statements;
using Twice.Compiler.Backend.AST.Types;

namespace Twice.Compiler.Backend.AST;

public class TwiceVisitor : TwiceBaseVisitor<ASTNode>
{
    public override ASTNode VisitProg(TwiceParser.ProgContext context)
    {
        return Visit(context.chunk());
    }
    
    public override ASTNode VisitChunk(TwiceParser.ChunkContext context)
    {
        return new TwiceChunk(context.statement().Select(x => (IStatement)Visit(x)).ToList());
    }

    public override ASTNode VisitStatement(TwiceParser.StatementContext context)
    {
        return Visit(context.children[0]);
    }

    public override ASTNode VisitExpressionNumber(TwiceParser.ExpressionNumberContext context)
    {
        int intAttempt;
        if (int.TryParse(context.NUMBER().GetText(), out intAttempt))
            return new IntExpression(intAttempt);

        float floatAttempt;
        if (float.TryParse(context.NUMBER().GetText(), CultureInfo.InvariantCulture, out floatAttempt))
            return new FloatExpression(floatAttempt);

        throw new NotImplementedException();
    }

    public override ASTNode VisitDeclarationStatement(TwiceParser.DeclarationStatementContext context)
    {
        return new DeclarationStatement(context.LET() == null, context.IDENTIFIER().GetText(), (IExpression)Visit(context.expression()));
    }

    public override ASTNode VisitExternFunctionDefinition(TwiceParser.ExternFunctionDefinitionContext context)
    {
        bool vararg = context.VARARG() != null;
        string functionName = context.IDENTIFIER().GetText();

        IType functionType = (IType)Visit(context.type());
        
        return new ExternFunctionStatement(vararg, functionName, functionType);
    }

    public override ASTNode VisitTypeBasic(TwiceParser.TypeBasicContext context)
    {
        switch (context.TYPE().GetText())
        {
            case "int":
                return new IntType();
            case "bool":
                return new BoolType();
            case "double":
                return new FloatType();
            case "string":
                return new StringType();
            case "void":
                return new VoidType();
        }

        throw new NotImplementedException();
    }

    public override ASTNode VisitStatementIf(TwiceParser.StatementIfContext context)
    {

        IExpression expression = (IExpression)Visit(context.expression());
        IStatement statement = (IStatement)Visit(context.statement()[0]);
        IStatement? elseStatement = context.ELSE() != null ? (IStatement)Visit(context.statement()[1]) : null;

        return new IfStatement(expression, statement, elseStatement);
    }

    public override ASTNode VisitStatementWhile(TwiceParser.StatementWhileContext context)
    {
        return new WhileStatement((IExpression)Visit(context.expression()), 
            (IStatement)Visit(context.statement()));
    }

    public override ASTNode VisitStatementFor(TwiceParser.StatementForContext context)
    {
        return new ForStatement(context.forLoop().IDENTIFIER().GetText(), 
            context.forLoop().BANG() != null, 
            (IExpression)Visit(context.forLoop().expression()), 
            (IStatement)Visit(context.forLoop().statement()));
        
    }

    public override ASTNode VisitStatementBlock(TwiceParser.StatementBlockContext context)
    {
        TwiceChunk chunk = (TwiceChunk)Visit(context.chunk());
        return new BlockStatement(chunk.Statements);
    }

    public override ASTNode VisitStatementFunctionDefinition(TwiceParser.StatementFunctionDefinitionContext context)
    {
        bool async = context.ASYNC() != null;
        string functionName = context.IDENTIFIER().GetText();
        IType returnType = (IType)Visit(context.functionType().type());

        List<FunctionArgument> arguments = new List<FunctionArgument>();

        for (int i = 0; i < context.functionType().functionArgs().IDENTIFIER().Length; i++)
        {
            arguments.Add(new FunctionArgument((IType)Visit(context.functionType().functionArgs().type(i)),context.functionType().functionArgs().IDENTIFIER(i).GetText()));
        }
        
        ASTNode innerNode = context.statement() == null ? Visit(context.expression()) : Visit(context.statement());
        
        return new FunctionDefinitionStatement(async, functionName, arguments, returnType, innerNode);
    }

    public override ASTNode VisitTypeFunction(TwiceParser.TypeFunctionContext context)
    {

        IType returnType = (IType)Visit(context.type(0));
        List<IType> arguments = new List<IType>();

        if (context.type().Length > 1)
        {
            for (int i = 1; i < context.type().Length; i++)
            {
                arguments.Add((IType)Visit(context.type(i)));
            }
        }

        return new FunctionType(returnType, arguments);
    }

    public override ASTNode VisitTypeGeneric(TwiceParser.TypeGenericContext context)
    {
        switch (context.GENERIC().GetText())
        {
            case "promise":
                return new PromiseType((IType)Visit(context.type()));
            case "channel":
                return new ChannelType((IType)Visit(context.type()));
            case "array":
                return new ArrayType((IType)Visit(context.type()));
        }

        throw new NotImplementedException();
    }

    public override ASTNode VisitFunctionCallStatement(TwiceParser.FunctionCallStatementContext context)
    {
        return new FunctionCallStatement(
            context.functionCall().IDENTIFIER().GetText(),
            context.functionCall().type() != null ? (IType)Visit(context.functionCall().type()) : null,
            context.functionCall().expression().Select(x => (IExpression)Visit(x)).ToList());
    }

    public override ASTNode VisitChannelPushStatement(TwiceParser.ChannelPushStatementContext context)
    {
        return new ChannelPushStatement((IExpression)Visit(context.expression(1)), (IExpression)Visit(context.expression(0)));
    }

    public override ASTNode VisitReturnStatement(TwiceParser.ReturnStatementContext context)
    {
        return new ReturnStatement(context.expression() != null ? (IExpression)Visit(context.expression()) : null);
    }

    public override ASTNode VisitAssignmentStatement(TwiceParser.AssignmentStatementContext context)
    {
        return new AssignmentStatement(context.IDENTIFIER().GetText(), (IExpression)Visit(context.expression()));
    }

    public override ASTNode VisitExpressionBool(TwiceParser.ExpressionBoolContext context)
    {
        return new BoolExpression(context.FALSE() == null);
    }

    public override ASTNode VisitExpressionAdd(TwiceParser.ExpressionAddContext context)
    {
        IExpression left = (IExpression)Visit(context.expression(0));
        IExpression right = (IExpression)Visit(context.expression(1));

        return new BinOpExpression(BinaryOperation.Addition, left, right);
    }

    public override ASTNode VisitExpressionModulo(TwiceParser.ExpressionModuloContext context)
    {
        IExpression left = (IExpression)Visit(context.expression(0));
        IExpression right = (IExpression)Visit(context.expression(1));

        return new BinOpExpression(BinaryOperation.Remainder, left, right);
    }

    public override ASTNode VisitExpressionMultiply(TwiceParser.ExpressionMultiplyContext context)
    {
        IExpression left = (IExpression)Visit(context.expression(0));
        IExpression right = (IExpression)Visit(context.expression(1));

        return new BinOpExpression(BinaryOperation.Multiplication, left, right);
    }

    public override ASTNode VisitAwaitStatement(TwiceParser.AwaitStatementContext context)
    {
        return new AwaitStatement((IExpression)Visit(context.expression()));
    }

    public override ASTNode VisitAwaitExpression(TwiceParser.AwaitExpressionContext context)
    {
        return new AwaitExpression((IExpression)Visit(context.expression()));
    }

    public override ASTNode VisitExpressionFunctionCall(TwiceParser.ExpressionFunctionCallContext context)
    {
        return new FunctionCallExpression(
            context.functionCall().IDENTIFIER().GetText(),
            context.functionCall().type() != null ? (IType)Visit(context.functionCall().type()) : null,
            context.functionCall().expression().Select(x => (IExpression)Visit(x)).ToList());
    }

    public override ASTNode VisitExpressionNegate(TwiceParser.ExpressionNegateContext context)
    {
        return new NegateExpression((IExpression)Visit(context.expression()));
    }

    public override ASTNode VisitExpressionParenthesis(TwiceParser.ExpressionParenthesisContext context)
    {
        return (IExpression)Visit(context.expression());
    }

    public override ASTNode VisitExpressionLambda(TwiceParser.ExpressionLambdaContext context)
    {
        bool async = context.ASYNC() != null;
        IType returnType = (IType)Visit(context.functionType().type());

        List<FunctionArgument> arguments = new List<FunctionArgument>();

        for (int i = 0; i < context.functionType().functionArgs().IDENTIFIER().Length; i++)
        {
            arguments.Add(new FunctionArgument((IType)Visit(context.functionType().functionArgs().type(i)),context.functionType().functionArgs().IDENTIFIER(i).GetText()));
        }
        
        ASTNode innerNode = context.statement() == null ? Visit(context.expression()) : Visit(context.statement());
        
        return new LambdaExpression(async, arguments, returnType, innerNode);
    }

    public override ASTNode VisitExpressionBlock(TwiceParser.ExpressionBlockContext context)
    {
        TwiceChunk chunk = (TwiceChunk)Visit(context.chunk());
        return new BlockExpression(chunk.Statements, (IExpression)Visit(context.expression()));
    }

    public override ASTNode VisitExpressionDivide(TwiceParser.ExpressionDivideContext context)
    {
        IExpression left = (IExpression)Visit(context.expression(0));
        IExpression right = (IExpression)Visit(context.expression(1));

        return new BinOpExpression(BinaryOperation.Division, left, right);
    }

    public override ASTNode VisitExpressionComparison(TwiceParser.ExpressionComparisonContext context)
    {
        IExpression left = (IExpression)Visit(context.expression(0));
        IExpression right = (IExpression)Visit(context.expression(1));

        BinaryOperation operation;

        if (context.comp().POINTY_OPEN() != null)
        {
            operation = BinaryOperation.LT;
        } else if (context.comp().POINTY_CLOSE() != null)
        {
            operation = BinaryOperation.GT;
        }
        else
        {
            switch (context.comp().COMP().GetText()[0])
            {
                case '<':
                    operation = BinaryOperation.LTE;
                    break;
                case '>':
                    operation = BinaryOperation.GTE;
                    break;
                case '!':
                    operation = BinaryOperation.Inequality;
                    break;
                case '=':
                    operation = BinaryOperation.Equality;
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        return new BinOpExpression(operation, left, right);
    }

    public override ASTNode VisitExpressionSubtract(TwiceParser.ExpressionSubtractContext context)
    {
        IExpression left = (IExpression)Visit(context.expression(0));
        IExpression right = (IExpression)Visit(context.expression(1));

        return new BinOpExpression(BinaryOperation.Subtraction, left, right);
    }

    public override ASTNode VisitExpressionArrayLiteral(TwiceParser.ExpressionArrayLiteralContext context)
    {
        return new ArrayLiteralExpression(context.expression().Select(x => (IExpression)Visit(x)).ToList());
    }

    public override ASTNode VisitExpressionAnd(TwiceParser.ExpressionAndContext context)
    {
        IExpression left = (IExpression)Visit(context.expression(0));
        IExpression right = (IExpression)Visit(context.expression(1));

        return new BinOpExpression(BinaryOperation.LogicalAnd, left, right);
    }

    public override ASTNode VisitExpressionUntil(TwiceParser.ExpressionUntilContext context)
    {
        IExpression left = (IExpression)Visit(context.expression(0));
        IExpression right = (IExpression)Visit(context.expression(1));

        return new BinOpExpression(BinaryOperation.ArrayRange, left, right);
    }

    public override ASTNode VisitExpressionString(TwiceParser.ExpressionStringContext context)
    {
        return new StringExpression(context.STRING().GetText()[1..^1]);
    }

    public override ASTNode VisitExpressionOr(TwiceParser.ExpressionOrContext context)
    {
        IExpression left = (IExpression)Visit(context.expression(0));
        IExpression right = (IExpression)Visit(context.expression(1));

        return new BinOpExpression(BinaryOperation.LogicalOr, left, right);
    }

    public override ASTNode VisitExpressionArrayAccess(TwiceParser.ExpressionArrayAccessContext context)
    {
        return new ArrayAccessExpression((IExpression)Visit(context.expression(0)), 
            (IExpression)Visit(context.expression(1)));
    }

    public override ASTNode VisitExpressionChannelLoad(TwiceParser.ExpressionChannelLoadContext context)
    {
        return new ChannelLoadExpression((IExpression)Visit(context.expression()));
    }

    public override ASTNode VisitExpressionVariable(TwiceParser.ExpressionVariableContext context)
    {
        return new VariableExpression(context.IDENTIFIER().GetText());
    }
}