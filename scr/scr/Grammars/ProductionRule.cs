using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using MathParsing.Extensions;

namespace MathParsing.Grammars;

public sealed record ProductionRule
{
    public NonterminalSymbol Nonterminal { get; private set; }

    public ImmutableList<GrammarSymbol> Replacement { get; private set; }

    [ExcludeFromCodeCoverage]
    public override string? ToString()
        => $"{Nonterminal} -> {StringExt.FromList(Replacement)}";

    public ProductionRule(NonterminalSymbol nonterminal, GrammarSymbol[] replacement)
    {
        Nonterminal = nonterminal;
        Replacement = replacement.ToImmutableList();
    }
}