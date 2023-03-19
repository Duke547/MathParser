using System.Text.RegularExpressions;
using MathParsing.Extensions;

namespace MathParsing.Lexing;

public static class Lexer
{
    static Token? ConsumeNextToken(ref string expression, IEnumerable<TokenPattern> tokenPatterns)
    {
        foreach (var tokenPattern in tokenPatterns)
        {
            var whitespacePattern = @"\s*";
            var match = Regex.Match(expression, $"{whitespacePattern}{tokenPattern.Pattern}{whitespacePattern}");

            if (match.Success && match.Index == 0)
            {
                expression = expression.Substring(match.Length);

                return new Token(tokenPattern.Description, match.Value.RemoveWhitespace());
            }
        }

        return null;
    }

    public static Token[] Tokenize(string expression, IEnumerable<TokenPattern> tokenPatterns)
    {
        var tokens = new List<Token>();

        while (expression.Length > 0)
        {
            var token = ConsumeNextToken(ref expression, tokenPatterns);

            if (token != null)
                tokens.Add(token);
            else
                throw new ArgumentException($"Unrecognized symbol at '{expression}'");
        }

        return tokens.ToArray();
    }
}