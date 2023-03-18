namespace MathParsing.Grammars;

public sealed record GrammarSymbol
{
    public string Description { get; private set; }

    public bool Terminal { get; private set; }

    public override string? ToString() => Description;
    
    public GrammarSymbol(string description, bool terminal = false)
    {
        Description = description;
        Terminal    = terminal;
    }
}