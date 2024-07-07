namespace Twice.Compiler.Backend.AST.Statements;

public class AwaitStatement(IExpression inner) : IStatement
{
    public IExpression Inner = inner;
}