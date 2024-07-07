using System.Collections.Generic;

namespace TypedAST;

public class VariableStack<T> where T : class
{

    private VariableStack<T>? Upper;

    private Dictionary<string, T> Scope = new();
    
    public VariableStack()
    {
        
    }

    public VariableStack(VariableStack<T> upper)
    {
        Upper = upper;
    }

    public T? Lookup(string key)
    {
        // First check current scope, then check upper scope if available
        return Scope.TryGetValue(key, out var lookup) ? lookup : Upper?.Lookup(key);
    }

    public bool IsDefined(string key)
    {
        return Lookup(key) != null;
    }

    public bool SetValue(string key, T value)
    {
        if (!Scope.ContainsKey(key))
        {
            Scope[key] = value;
            return true;
        }

        return false;
    }

    public VariableStack<T>? GetUpper()
    {
        return Upper;
    }
    
}