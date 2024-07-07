using Twice.Compiler.Backend.AST.Types;

namespace Twice.Compiler.Backend.AST.Expressions;

public class VariableExpression(string value) : IExpression
{
    public string Variable = value;
}