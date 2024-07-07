using Twice.Compiler.Backend.AST.Types;

namespace Twice.Compiler.Backend.AST.Statements;

public class ExternFunctionStatement(bool vararg, string functionName, IType fnType) : IStatement
{

    public bool Vararg = vararg;
    public string FunctionName = functionName;
    public IType FunctionType = fnType;
}