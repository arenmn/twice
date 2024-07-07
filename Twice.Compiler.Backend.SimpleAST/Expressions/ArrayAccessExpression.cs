namespace Twice.Compiler.Backend.AST.Expressions;

public class ArrayAccessExpression(IExpression array, IExpression index) : IExpression
{
    public IExpression Array = array;
    public IExpression Index = index;
}