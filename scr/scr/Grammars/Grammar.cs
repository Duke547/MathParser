using System.Collections.Immutable;

namespace MathParsing.Grammars;

public record struct Grammar
{
    public NonterminalSymbol Start{ get; }

    public ImmutableList<ProductionRule> Rules { get; }

    public Grammar(NonterminalSymbol start, IEnumerable<ProductionRule> rules)
    {
        Start = start;
        Rules = rules.ToImmutableList();
    }
}