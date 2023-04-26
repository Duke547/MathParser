namespace MathParsing;

internal record BracketToken : IMathToken
{
    public bool Left { get; init; }

    public override string? ToString()
        => Left ? "(" : ")";

    public BracketToken(bool left)
        => Left = left;
}