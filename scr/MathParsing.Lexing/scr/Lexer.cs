using System.Text.RegularExpressions;
using MathParsing.Extensions;

namespace MathParsing.Lexing;

public static class Lexer
{
    private static Token? ConsumeNextToken(ref string expression, IEnumerable<TokenPattern> tokenPatterns)
    {
        foreach (var tokenPattern in tokenPatterns)
        {
            var match = Regex.Match(expression, tokenPattern.Pattern);

            if (match.Success && match.Index == 0)
            {
                expression = expression.Substring(match.Length);

                return new Token(tokenPattern.Description, match.Value);
            }
        }

        return null;
    }

    public static Token[] Tokenize(string expression, IEnumerable<TokenPattern> tokenPatterns)
    {
        var tokens = new List<Token>();
        expression = expression.RemoveWhitespace();

        while (expression.Length > 0)
        {
            var token = ConsumeNextToken(ref expression, tokenPatterns);

            if (token != null)
                tokens.Add(token);
            
            else if (token == null && expression.Length > 0)
                throw new InvalidOperationException($"Unrecognized pattern at ^{expression}");
        }

        return tokens.ToArray();
    }
}