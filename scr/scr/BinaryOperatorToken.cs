using System.Diagnostics.CodeAnalysis;

namespace MathParsing;

internal record BinaryOperatorToken : IMathToken
{
    public string Symbol { get; init; }

    public Func<decimal, decimal, decimal> BinaryOperation { get; init; }

    [ExcludeFromCodeCoverage]
    public override string ToString()
        => Symbol.ToString();

    public BinaryOperatorToken(string symbol, Func<decimal, decimal, decimal> binaryOperation)
    {
        Symbol          = symbol;
        BinaryOperation = binaryOperation;
    }
}