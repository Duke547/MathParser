using System.Diagnostics.CodeAnalysis;

namespace MathParsing;

internal record NumberToken : IMathToken
{
    public decimal Value { get; init; }

    [ExcludeFromCodeCoverage]
    public override string ToString()
        => Value.ToString();

    public NumberToken(decimal value)
        => Value = value;
}