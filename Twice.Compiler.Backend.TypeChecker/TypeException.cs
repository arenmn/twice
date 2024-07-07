using System;

namespace TypedAST;

public class TypeException : Exception
{
    public TypeException(string message) : base(message)
    {
    }
}