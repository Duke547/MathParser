namespace MathParsing;

internal record UnaryOperation
{
    public string Symbol { get; init; }

    public Func<decimal, decimal> Operation { get; init; }

    public override string ToString() => Symbol;

    public UnaryOperation(string symbol, Func<decimal, decimal> operation)
    {
        Symbol     = symbol;
        Operation  = operation;
    }
}