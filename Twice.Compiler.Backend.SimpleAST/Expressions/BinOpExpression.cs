namespace Twice.Compiler.Backend.AST.Expressions;

public enum BinaryOperation
{
    Addition,
    Subtraction,
    Multiplication,
    Division,
    LogicalAnd,
    LogicalOr,
    LT,
    LTE,
    GT,
    GTE,
    Equality,
    Inequality,
    Remainder,
    ArrayRange
}

public class BinOpExpression(BinaryOperation operation, IExpression left, IExpression right) : IExpression
{
    public BinaryOperation Operation = operation;
    public IExpression Left = left;
    public IExpression Right = right;
}