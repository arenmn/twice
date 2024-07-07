namespace Twice.Compiler.Backend.AST.Types;

public class FloatType : IType
{
    public string Name()
    {
        return "double";
    }
}