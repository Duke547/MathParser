using System.Text.RegularExpressions;
using MathParsing.Extensions;
using MathParsing.Grammars;

namespace MathParsing.Lexing;

internal static class Lexer
{
    static Token ConsumeNextToken(string expression, ref int currentIndex, IEnumerable<TokenPattern> tokenPatterns)
    {
        foreach (var tokenPattern in tokenPatterns)
        {
            var whitespacePattern = @"\s*";
            var match             = Regex.Match(expression.Substring(currentIndex), $"{whitespacePattern}{tokenPattern.Pattern}{whitespacePattern}");
            var matchIndex        = match.Index;

            if (match.Success && matchIndex == 0)
            {
                var token = new Token(tokenPattern.Description, tokenPattern.Subset, match.Value.RemoveWhitespace(), currentIndex);

                currentIndex += match.Length;

                return token;
            }
        }

        var unrecognizedToken = expression.Substring(currentIndex, 1);

        throw new IndexedTokenException(unrecognizedToken, currentIndex, $"Unrecognized token '{unrecognizedToken}' at position {currentIndex}.");
    }

    public static Token[] Tokenize(string expression, IEnumerable<TokenPattern> tokenPatterns)
    {
        var tokens       = new List<Token>();
        var currentIndex = 0;

        while (currentIndex < expression.Length)
        {
            var token = ConsumeNextToken(expression, ref currentIndex, tokenPatterns);

            tokens.Add(token);
        }

        return tokens.ToArray();
    }
}