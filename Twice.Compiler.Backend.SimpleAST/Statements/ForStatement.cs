using Twice.Compiler.Backend.AST.Types;

namespace Twice.Compiler.Backend.AST.Statements;

public class ForStatement(string innerVariable, bool parallel, IExpression expression, IStatement mainStatement) : IStatement
{
    public bool Parallel = parallel;
    public string InnerVariable = innerVariable;
    public IExpression Expression = expression;
    public IStatement Statement = mainStatement;

}