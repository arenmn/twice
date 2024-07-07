namespace Twice.Compiler.Backend.AST.Types;

public class ArrayType(IType type) : IType
{
    public readonly IType InnerType = type;

    public string Name()
    {
        return $"array<{InnerType.Name()}>";
    }

    public string GenericName()
    {
        return "array";
    }
}