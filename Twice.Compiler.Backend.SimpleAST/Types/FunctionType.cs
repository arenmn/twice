using System;
using System.Collections.Generic;
using System.Linq;

namespace Twice.Compiler.Backend.AST.Types;

public class FunctionType(IType returnType, List<IType> argumentTypes) : IType
{
    public IType ReturnType = returnType;
    public List<IType> ArgumentTypes = argumentTypes;

    public string Name()
    {
        return $"{ReturnType.Name()}({String.Join(',', ArgumentTypes.Select(x => x.Name()))})";
    }

    public string GenericName()
    {
        return "function";
    }
}