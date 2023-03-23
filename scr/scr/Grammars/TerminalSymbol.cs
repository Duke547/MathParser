namespace MathParsing.Grammars;

public sealed record TerminalSymbol : GrammarSymbol
{
    public TerminalSymbol(string description) : base(description) { }
}