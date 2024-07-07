namespace Twice.Compiler.Backend.AST.Types;

public class StringType : IType
{
    public string Name()
    {
        return "string";
    }
}