using MathParsing.Lexing;

namespace MathParsing;

internal static class MathTokenConverter
{
    private static NumberToken ConvertToNumberToken(Token token)
    {
        try
        {
            var value = decimal.Parse(token.Text);

            return new(value);
        }
        catch (FormatException exception)
        {
            throw new ArgumentException($"'{token}' does not represent a number.", exception);
        }
    }

    private static Func<decimal, decimal, decimal> ConvertToBinaryOperation(string symbol)
    {
        if (symbol == "+")
            return (l, r) => l + r;

        if (symbol == "*")
            return (l, r) => l * r;

        throw new ArgumentException($"'{symbol}' does not represent a known binary operator.");
    }

    private static BinaryOperatorToken ConvertToBinaryOperatorToken(Token token)
    {
        var operation = ConvertToBinaryOperation(token.Text);

        return new(token.Text, operation);
    }

    public static IMathToken Convert(Token token)
    {
        if (token.Description == "number")
            return ConvertToNumberToken(token);
        
        return ConvertToBinaryOperatorToken(token);
    }

    public static IMathToken[] Convert(IEnumerable<Token> tokens)
    {
        return tokens.Select(token => Convert(token))
                     .ToArray();
    }
}