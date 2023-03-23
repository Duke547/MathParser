namespace MathParsing.Grammars;

public sealed record NonterminalSymbol : GrammarSymbol
{
    public NonterminalSymbol(string description) : base(description) { }
}