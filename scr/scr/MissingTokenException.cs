namespace MathParsing;

public class MissingTokenException : TokenException
{
    public MissingTokenException(string token) :
        base(token, $"Missing '{token}'.") { }
}