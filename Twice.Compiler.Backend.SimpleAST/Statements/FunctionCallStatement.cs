using System.Collections.Generic;
using Twice.Compiler.Backend.AST.Types;

namespace Twice.Compiler.Backend.AST.Statements;

public class FunctionCallStatement(string functionName, IType? genericType, List<IExpression> arguments) : IStatement
{
    public string FunctionName = functionName;
    public IType? GenericType = genericType;
    public List<IExpression> Arguments = arguments;
    
}