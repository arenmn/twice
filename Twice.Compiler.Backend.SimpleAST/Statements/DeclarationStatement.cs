using Twice.Compiler.Backend.AST.Types;

namespace Twice.Compiler.Backend.AST.Statements;

public class DeclarationStatement(bool constant, string name, IExpression value) : IStatement
{

    public bool Constant = constant;
    public string Name = name;
    public IExpression Value = value;
    
}