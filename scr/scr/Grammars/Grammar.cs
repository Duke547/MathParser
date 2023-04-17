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
            var rule      = Rules.Find(r => r.Token == token.Description);
            var nextToken = tokens.ElementAtOrDefault(index + 1);

            if (rule is null)
                throw new UndefinedTokenException(token.Text);

            if (index == 0 && !rule.CanStart)
                throw new UnexpectedTokenException(token.Text);

            if (nextToken is null && !rule.CanEnd)
                throw new UnexpectedTokenException(token.Text);

            if (nextToken is not null && !rule.Adjacents.Select(a => a).Contains(nextToken.Description))
                throw new UnexpectedTokenException(token.Text);
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