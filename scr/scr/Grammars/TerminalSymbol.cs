using System.Diagnostics.CodeAnalysis;

namespace MathParsing.Grammars;

internal sealed record TerminalSymbol : GrammarSymbol
{
    [ExcludeFromCodeCoverage]
    public override string? ToString() => base.ToString();

    public TerminalSymbol(string description) : base(description) { }
}