using Twice.Compiler.Backend.AST.Types;

namespace Twice.Compiler.Backend.AST.Expressions;

public class ChannelLoadExpression(IExpression channel) : IExpression
{
    public IExpression Channel = channel;
}