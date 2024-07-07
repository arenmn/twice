using System.Collections.Generic;
using Twice.Compiler.Backend.AST.Types;

namespace Twice.Compiler.Backend.AST.Expressions;

public class ArrayLiteralExpression(List<IExpression> items): IExpression
{
    public List<IExpression> Items = items;
}