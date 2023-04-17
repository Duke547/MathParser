namespace MathParsing.Grammars;

internal class UndefinedTokenException : TokenException
{
    public UndefinedTokenException(string token)
        : base(token, $"Undefined token '{token}'.") { }
}