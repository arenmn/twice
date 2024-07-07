using Twice.Compiler.Backend.AST.Types;

namespace Twice.Compiler.Backend.AST.Expressions;

public class StringExpression : IExpression
{
    
    public string Value;

    public StringExpression(string value)
    {
        Value = value.Replace("\\n", "\n").Replace("\\t", "\t").Replace("\\r", "\r");
    }

}