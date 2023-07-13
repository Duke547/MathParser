namespace MathParsing;

internal record UnaryOperatorToken : IMathToken
{
    public UnaryOperation UnaryOperation { get; init; }

    public string Symbol => UnaryOperation.Symbol;

    public override string ToString()
        => Symbol.ToString();

    public UnaryOperatorToken(UnaryOperation unaryOperation)
        => UnaryOperation = unaryOperation;
}