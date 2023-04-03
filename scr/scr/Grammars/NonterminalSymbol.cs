using System.Diagnostics.CodeAnalysis;

namespace MathParsing.Grammars;

internal sealed record NonterminalSymbol : GrammarSymbol
{
    [ExcludeFromCodeCoverage]
    public override string? ToString() => base.ToString();

    public NonterminalSymbol(string description) : base(description) { }
}