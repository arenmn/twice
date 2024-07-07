using System.Collections.Generic;
using Twice.Compiler.Backend.AST.Statements;
using Twice.Compiler.Backend.AST.Types;

namespace Twice.Compiler.Backend.AST.Expressions;

public class LambdaExpression(bool async, List<FunctionArgument> arguments, IType returnType, ASTNode inner) : IStatement
{

    public bool Async = async;
    public List<FunctionArgument> Arguments = arguments;
    public IType ReturnType = returnType;
    public ASTNode InnerNode = inner;
    
}