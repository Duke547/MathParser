namespace MathParsing.Grammars;

internal class UnexpectedTokenException : TokenException
{
    public UnexpectedTokenException(string token)
        : base(token, $"Unexpected token '{token}'.") { }
}