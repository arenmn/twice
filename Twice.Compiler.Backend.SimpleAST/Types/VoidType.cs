namespace Twice.Compiler.Backend.AST.Types;

public class VoidType : IType
{
    public string Name()
    {
        return "void";
    }
}