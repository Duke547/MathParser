using System.Text.RegularExpressions;
using MathParsing.Extensions;
using MathParsing.Grammars;

namespace MathParsing.Lexing;

internal static class Lexer
{
    private static string ConcatenatePatterns(IEnumerable<TokenPattern> tokenPatterns)
    {
        var pattern = "";

        foreach (var tokenPattern in tokenPatterns)
        {
            if (pattern != "")
                pattern += "|";

            var desc = tokenPattern.Description.Replace(" ", "_");

            pattern += $"(?'{desc}'{tokenPattern.Pattern})";
        }

        return pattern;
    }

    private static string RemoveMatches(string expression, MatchCollection matches)
    {
        foreach (var match in matches.Cast<Match>())
            expression = expression.RemoveFirst(match.Value);

        return expression;
    }

    private static void Validate(string expression, MatchCollection matches)
    {
        var remaining = RemoveMatches(expression.RemoveWhitespace(), matches);

        if (remaining != "")
        {
            var unmatchedToken = remaining.Substring(0, 1);

            throw new TokenException(unmatchedToken, $"Unrecognized token '{unmatchedToken}'.");
        }
    }

    private static Token[] ConvertToTokens(MatchCollection matches, IEnumerable<TokenPattern> tokenPatterns)
    {
        var tokens = new List<Token>();

        foreach (var match in matches.Cast<Match>())
        {
            var description = match.Groups.Values.First(m => m.Success && m.Name != "0"  ).Name.Replace('_', ' ');
            var subset      = tokenPatterns      .First(p => p.Description == description).Subset;

            tokens.Add(new(description, subset, match.Value));
        }

        return tokens.ToArray();
    }

    /// <summary>
    /// Converts the expression into a collection of lexical tokens based on the given token patterns.
    /// </summary>
    /// <param name="expression">The expression to convert.</param>
    /// <param name="tokenPatterns">The token patterns to use for conversion.</param>
    /// <returns>The collection of tokens extracted from the given expression.</returns>
    public static Token[] Tokenize(string expression, IEnumerable<TokenPattern> tokenPatterns)
    {
        var pattern = ConcatenatePatterns(tokenPatterns);
        var matches = Regex.Matches(expression, pattern);

        Validate(expression, matches);

        return ConvertToTokens(matches, tokenPatterns);
    }
}