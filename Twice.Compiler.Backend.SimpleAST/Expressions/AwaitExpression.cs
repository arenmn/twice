namespace Twice.Compiler.Backend.AST.Expressions;

public class AwaitExpression(IExpression inner) : IExpression
{
    public IExpression Inner = inner;
}