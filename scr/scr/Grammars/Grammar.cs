using System.Collections.Immutable;
using MathParsing.Lexing;

namespace MathParsing.Grammars;

internal sealed record Grammar
{
    public ImmutableList<Rule> Rules { get; private set; }

    public void Validate(IEnumerable<Token> tokens)
    {
        foreach (var token in tokens)
        {
            var index     = Array.IndexOf(tokens.ToArray(), token);
            var position  = token.Position;
            var text      = token.Text;
            var rule      = Rules.Find(r => r.Token == token.Description);
            var nextToken = tokens.ElementAtOrDefault(index + 1);

            if (rule is null)
                throw new IndexedTokenException(text, position, $"Undefined token '{text}' at position {position}.");

            if ((index == 0 && !rule.CanStart) || (nextToken is null && !rule.CanEnd))
                throw new IndexedTokenException(text, position, $"Unexpected token '{text}' at position {position}.");

            if (nextToken is not null && !rule.Adjacents.Select(a => a).Contains(nextToken.Description))
            {
                var nextText     = nextToken.Text;
                var nextPosition = nextToken.Position;

                throw new IndexedTokenException(nextText, nextPosition, $"Unexpected token '{nextText}' at position {nextPosition}.");
            }
        }
    }

    private static void ValidateRules(IEnumerable<Rule> rules)
    {
        foreach (var rule in rules)
        {
            var tokens = rules.Select(r => r.Token);
            
            if (tokens.Count(t => t == rule.Token) > 1)
                throw new MultipleRulesForSameTokenException(rule.Token);
        }
    }

    public Grammar(IEnumerable<Rule> rules)
    {
        ValidateRules(rules);

        Rules = rules.ToImmutableList();
    }
}