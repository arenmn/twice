namespace Twice.Compiler.Backend.AST.Types;

public class PromiseType(IType type) : IType
{
    public readonly IType InnerType = type;

    public string Name()
    {
        return $"promise<{InnerType.Name()}>";
    }

    public string GenericName()
    {
        return "promise";
    }
}