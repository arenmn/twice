using System.Collections.Generic;
using Twice.Compiler.Backend.AST.Types;

namespace Twice.Compiler.Backend.AST.Expressions;

public class FunctionCallExpression(string functionName, IType? genericType, List<IExpression> arguments) : IExpression 
{
    public string FunctionName = functionName;
    public IType? GenericType = genericType;
    public List<IExpression> Arguments = arguments;
    
}