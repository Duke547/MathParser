namespace MathParsing;
public class IndexedTokenException : TokenException
{
    public int Position { get; init; }

    public IndexedTokenException(string token, int position, string message) : base(token, message)
        => Position = position;
}