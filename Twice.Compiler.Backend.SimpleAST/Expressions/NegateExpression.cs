using Twice.Compiler.Backend.AST.Types;

namespace Twice.Compiler.Backend.AST.Expressions;

public class NegateExpression(IExpression expression) : IExpression
{
    public IExpression Expression = expression;
}