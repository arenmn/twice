using System.Collections.Generic;
using Twice.Compiler.Backend.AST.Types;

namespace Twice.Compiler.Backend.AST.Statements;

public class BlockStatement(List<IStatement> statements) : IStatement
{

    public List<IStatement> Statements = statements;
    
}