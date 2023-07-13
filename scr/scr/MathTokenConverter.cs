using MathParsing.Grammars;
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
            throw new TokenException(token.ToString(), $"'{token}' does not represent a number.", exception);
        }
    }

    private static BinaryOperation GetBinaryOperation(Token token)
    {
        var symbol      = token.Text;
        var description = token.Description;

        if (description == "plus")
            return new(symbol, (l, r) => l + r, 0);

        if (description == "minus")
            return new(symbol, (l, r) => l - r, 0);

        if (description == "multiply")
            return new(symbol, (l, r) => l * r, 1);

        if (description == "divide")
            return new(symbol, (l, r) => l / r, 1);

        if (description == "remainder")
            return new(symbol, (l, r) => l % r, 1);

        throw new TokenException(symbol, $"'{symbol}' does not represent a known binary operator.");
    }

    private static UnaryOperation GetUnaryOperation(Token token)
    {
        var symbol      = token.Text;
        var description = token.Description;

        if (description == "plus")
            return new(symbol, (v) => v);

        if (description == "minus")
            return new(symbol, (v) => -v);

        throw new TokenException(symbol, $"'{symbol}' does not represent a known unary operator.");
    }

    static IMathToken ConvertToOperatorToken(Token token, Token? previous)
    {
        if (previous?.Description == "number" || previous?.Subset == "right bracket")
            return ConvertToBinaryOperatorToken(token);
        else
            return ConvertToUnaryOperatorToken(token);
    }

    private static BinaryOperatorToken ConvertToBinaryOperatorToken(Token token)
    {
        var operation = GetBinaryOperation(token);

        return new(operation);
    }

    private static UnaryOperatorToken ConvertToUnaryOperatorToken(Token token)
        => new(GetUnaryOperation(token));

    private static BracketToken ConvertToBracketToken(Token token)
    {
        if (token.Subset == "left bracket")
            return new(true);
        else
            return new(false);
    }

    public static IMathToken Convert(Token token, Token? previous)
    {
        if (token.Description == "number")
            return ConvertToNumberToken(token);

        if (token.Subset == "operator")
            return ConvertToOperatorToken(token, previous);

        if (token.Subset.Contains("bracket"))
            return ConvertToBracketToken(token);

        throw new TokenException(token.ToString(), $"Unrecognized {token.Subset} '{token}'.");
    }

    public static IMathToken[] Convert(IEnumerable<Token> tokens)
    {
        var    mathTokens = new List<IMathToken>();
        Token? previous   = null;

        foreach (var token in tokens)
        {
            mathTokens.Add(Convert(token, previous));

            previous = token;
        }

        return mathTokens.ToArray();
    }
}