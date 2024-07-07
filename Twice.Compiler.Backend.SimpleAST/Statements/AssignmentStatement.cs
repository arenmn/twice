using Twice.Compiler.Backend.AST.Types;

namespace Twice.Compiler.Backend.AST.Statements;

public class AssignmentStatement(string name, IExpression value) : IStatement
{
    public string Name = name;
    public IExpression Value = value;
}