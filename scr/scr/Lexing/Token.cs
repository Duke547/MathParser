using System.Diagnostics.CodeAnalysis;

namespace MathParsing.Lexing;

public sealed record Token
{
    public string Description { get; private set; }

    public string Text { get; private set; }

    [ExcludeFromCodeCoverage]
    public override string? ToString() => $"{Text} ({Description})";

    public Token(string description, string text)
    {
        Description = description;
        Text        = text;
    }
}