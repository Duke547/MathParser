namespace MathParsing.Grammars;

internal class MultipleRulesForSameTokenException : TokenException
{
    public MultipleRulesForSameTokenException(string token)
        : base(token, $"Multiple rules defined for token '{token}'.") { }
}