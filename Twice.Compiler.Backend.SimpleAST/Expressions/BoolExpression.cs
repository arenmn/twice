using Twice.Compiler.Backend.AST.Types;

namespace Twice.Compiler.Backend.AST.Expressions;

public class BoolExpression(bool value) : IExpression
{
    public bool Value = value;
}