using System.Collections.Immutable;

namespace MathParsing.Grammars;

public sealed record Grammar
{
    public GrammarSymbol Start{ get; }

    public ImmutableList<ProductionRule> Rules { get; }

    public Grammar(GrammarSymbol start, IEnumerable<ProductionRule> rules)
    {
        Start = start;
        Rules = rules.ToImmutableList();
    }
}