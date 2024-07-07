using Twice.Compiler.Backend.AST.Types;

namespace Twice.Compiler.Backend.AST.Statements;

public class IfStatement(IExpression expression, IStatement mainStatement, IStatement? elseStatement) : IStatement
{
    public IExpression Expression = expression;
    public IStatement Statement = mainStatement;
    public IStatement? ElseStatement = elseStatement;

}