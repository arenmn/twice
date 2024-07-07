using Twice.Compiler.Backend.AST.Types;

namespace Twice.Compiler.Backend.AST.Expressions;

public class IntExpression(int value) : IExpression
{
    public int Value = value;
}