using System.Collections.Generic;

namespace Twice.Compiler.Backend.AST;

public class TwiceChunk(List<IStatement> statements) : ASTNode
{
    public List<IStatement> Statements = statements;
}