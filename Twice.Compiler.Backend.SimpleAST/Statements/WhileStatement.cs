using Twice.Compiler.Backend.AST.Types;

namespace Twice.Compiler.Backend.AST.Statements;

public class WhileStatement(IExpression expression, IStatement mainStatement) : IStatement
{
    public IExpression Expression = expression;
    public IStatement Statement = mainStatement;

}