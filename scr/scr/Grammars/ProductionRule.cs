using System.Collections.Immutable;
using MathParsing.Extensions;

namespace MathParsing.Grammars;

public sealed record ProductionRule
{
    public GrammarSymbol Nonterminal { get; private set; }

    public ImmutableList<GrammarSymbol> Replacement { get; private set; }

    public override string? ToString()
        => $"{Nonterminal} -> {StringExt.FromList(Replacement)}";

    public ProductionRule(GrammarSymbol nonterminal, GrammarSymbol[] replacement)
    {
        Nonterminal = nonterminal;
        Replacement = replacement.ToImmutableList();
    }
}