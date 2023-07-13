namespace MathParsing;

public class TokenException : Exception
{
    public string Token { get; private set; }

    public TokenException(string token, string? message) : base(message)
        => Token = token;

    public TokenException(string token, string? message, Exception? innerException) : base(message, innerException)
        => Token = token;
}