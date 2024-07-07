using Twice.Compiler.Backend.AST.Types;

namespace Twice.Compiler.Backend.AST.Statements;

public class ReturnStatement(IExpression? value) : IStatement
{
    public IExpression? Value = value;
}