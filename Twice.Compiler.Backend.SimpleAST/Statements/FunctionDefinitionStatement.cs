using System.Collections.Generic;
using Twice.Compiler.Backend.AST.Types;

namespace Twice.Compiler.Backend.AST.Statements;

public class FunctionDefinitionStatement(bool async, string functionName, List<FunctionArgument> arguments, IType returnType, ASTNode inner) : IStatement
{

    public bool Async = async;
    public string FunctionName = functionName;
    public List<FunctionArgument> Arguments = arguments;
    public IType ReturnType = returnType;
    public ASTNode InnerNode = inner;
    
}