using Twice.Compiler.Backend.AST.Types;

namespace Twice.Compiler.Backend.AST.Expressions;

public class FloatExpression(float value) : IExpression
{
    public float Value = value;
}