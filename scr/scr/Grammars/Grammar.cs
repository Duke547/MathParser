using System.Collections.Immutable;
using MathParsing.Lexing;

namespace MathParsing.Grammars;

internal sealed record Grammar
{
    public ImmutableList<Rule> Rules { get; private set; }

    public void Validate(IList<Token> tokens)
    {
        for (int i = 0; i < tokens.Count; i++)
        {
            var token     = tokens[i];
            var text      = token.Text;
            var rule      = Rules.Find(r => r.Token == token.Subset || r.Token == token.Description);
            var nextToken = tokens.ElementAtOrDefault(i + 1);

            if (rule is null)
                throw new TokenException(text, $"Undefined token '{text}'.");

            if ((i == 0 && !rule.CanStart) || (nextToken is null && !rule.CanEnd))
                throw new TokenException(text, $"Unexpected token '{text}'.");

            if (nextToken is not null && !rule.Adjacents.Select(a => a).Contains(nextToken.Subset)
                                      && !rule.Adjacents.Select(a => a).Contains(nextToken.Description))
            {
                var nextTokenText = nextToken.Text;

                throw new TokenException(nextTokenText, $"Unexpected token '{nextTokenText}'.");
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