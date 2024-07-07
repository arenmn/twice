namespace Twice.Compiler.Backend.AST.Types;

public class ChannelType(IType type) : IType
{
    public readonly IType InnerType = type;

    public string Name()
    {
        return $"channel<{InnerType.Name()}>";
    }

    public string GenericName()
    {
        return "channel";
    }
}