using Twice.Compiler.Backend.AST.Types;

namespace Twice.Compiler.Backend.AST.Statements;

public class ChannelPushStatement(IExpression channel, IExpression value) : IStatement
{
    public IExpression Channel = channel;
    public IExpression Value = value;

}