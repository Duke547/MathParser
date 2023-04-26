using System.Diagnostics.CodeAnalysis;

namespace MathParsing.Lexing;

internal sealed record Token
{
    public string Description { get; private set; }

    public string Subset { get; init; }

    public string Text { get; private set; }

    public int Position { get; init; }

    [ExcludeFromCodeCoverage]
    public override string? ToString() => $"{Text}";

    public Token(string description, string subset, string text, int position)
    {
        Description = description;
        Subset      = subset;
        Text        = text;
        Position    = position;
    }

    public Token(string description, string text, int position)
        : this(description, "", text, position) { }
}