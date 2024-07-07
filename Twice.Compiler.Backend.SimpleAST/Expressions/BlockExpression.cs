using System.Collections.Generic;

namespace Twice.Compiler.Backend.AST.Expressions;

public class BlockExpression(List<IStatement> statements, IExpression returnExpression) : IExpression
{

    public IExpression ReturnExpression = returnExpression;
    public List<IStatement> Statements = statements;
    
}