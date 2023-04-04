using System.Collections.Immutable;

namespace MathParsing.Grammars;

internal record Grammar
{
    public NonterminalSymbol Start{ get; }

    public List<ProductionRule> Rules { get; set; }

    public Grammar(NonterminalSymbol start, IEnumerable<ProductionRule> rules)
    {
        Start = start;
        Rules = rules.ToList();
    }
}