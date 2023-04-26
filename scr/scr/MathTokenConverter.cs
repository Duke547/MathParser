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

    private static BinaryOperation ConvertToBinaryOperation(Token token)
    {
        var symbol = token.Text;
        var subset = token.Subset;

        if (subset == "add")
            return new(symbol, (l, r) => l + r, 0);

        if (subset == "subtract")
            return new(symbol, (l, r) => l - r, 0);

        if (subset == "multiply")
            return new(symbol, (l, r) => l * r, 1);

        if (subset == "divide")
            return new(symbol, (l, r) => l / r, 1);

        if (subset == "remainder")
            return new(symbol, (l, r) => l % r, 1);

        throw new ArgumentException($"'{symbol}' does not represent a known binary operator.");
    }

    private static BinaryOperatorToken ConvertToBinaryOperatorToken(Token token)
    {
        var operation = ConvertToBinaryOperation(token);

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