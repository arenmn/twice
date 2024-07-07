namespace Twice.Compiler.Backend.AST.Types;

public interface IType : ASTNode
{
    string Name();

    string GenericName()
    {
        return Name();
    }
}