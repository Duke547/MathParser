namespace MathParsing;

internal record BinaryOperation
{
    public string Symbol { get; init; }

    public Func<decimal, decimal, decimal> Operation { get; init; }

    public int Precedence { get; init; }

    public override string? ToString() => Symbol;

    public BinaryOperation(string symbol, Func<decimal, decimal, decimal> operation, int precedence)
    {
        Symbol     = symbol;
        Operation  = operation;
        Precedence = precedence;
    }
}