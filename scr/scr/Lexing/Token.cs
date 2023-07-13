using System.Diagnostics.CodeAnalysis;

namespace MathParsing.Lexing;

internal sealed record Token
{
    public string Description { get; private set; }

    public string Subset { get; init; }

    public string Text { get; private set; }

    [ExcludeFromCodeCoverage]
    public override string ToString() => $"{Text}";

    public Token(string description, string subset, string text)
    {
        Description = description;
        Subset      = subset;
        Text        = text;
    }
}