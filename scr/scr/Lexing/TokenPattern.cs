using System.Diagnostics.CodeAnalysis;

namespace MathParsing.Lexing;

internal sealed record TokenPattern
{
    public string Description { get; private set; }
    
    public string Subset { get; init; }

    public string Pattern { get; private set; }

    [ExcludeFromCodeCoverage]
    public override string? ToString() => Description;

    public TokenPattern(string type, string subset, string pattern)
    {
        Description = type;
        Subset      = subset;
        Pattern     = pattern;
    }

    public TokenPattern(string type, string pattern)
        : this(type, "", pattern) { }
}