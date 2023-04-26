namespace MathParsing.Grammars;

public class TokenException : Exception
{
    public string Token { get; private set; }

    public TokenException(string token, string? message) : base(message)
        => Token = token;
}