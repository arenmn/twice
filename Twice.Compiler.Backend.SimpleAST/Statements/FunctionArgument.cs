using Twice.Compiler.Backend.AST.Types;

namespace Twice.Compiler.Backend.AST.Statements;

public class FunctionArgument(IType type, string name) : ASTNode
{
    public IType Type = type;
    public string Name = name;
}