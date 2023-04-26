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

    private static BinaryOperation ConvertToBinaryOperation(string symbol)
    {
        if (symbol == "+")
            return new(symbol, (l, r) => l + r, 0);

        if (symbol == "*")
            return new(symbol, (l, r) => l * r, 1);

        throw new ArgumentException($"'{symbol}' does not represent a known binary operator.");
    }

    private static BinaryOperatorToken ConvertToBinaryOperatorToken(Token token)
    {
        var operation = ConvertToBinaryOperation(token.Text);

        return new(operation);
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