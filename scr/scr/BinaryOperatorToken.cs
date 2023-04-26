namespace MathParsing;

internal record BinaryOperatorToken : IMathToken
{
    public BinaryOperation BinaryOperation { get; init; }

    public string Symbol => BinaryOperation.Symbol;

    public int Precedence => BinaryOperation.Precedence;

    public override string ToString()
        => Symbol.ToString();

    public BinaryOperatorToken(BinaryOperation binaryOperation)
        => BinaryOperation = binaryOperation;
}