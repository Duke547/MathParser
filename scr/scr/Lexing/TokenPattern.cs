using System.Diagnostics.CodeAnalysis;

namespace MathParsing.Lexing;

public sealed record TokenPattern
{
    public string Description { get; private set; }

    public string Pattern { get; private set; }

    [ExcludeFromCodeCoverage]
    public override string? ToString() => Description;

    public TokenPattern(string type, string pattern)
    {
        Description = type;
        Pattern     = pattern;
    }
}