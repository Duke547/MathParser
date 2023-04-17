using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using MathParsing.Extensions;
using MathParsing.Lexing;

namespace MathParsing.Grammars;

internal sealed record Rule
{
    public string Token { get; private set; }

    public bool CanStart { get; private set; }

    public bool CanEnd { get; private set; }

    public ImmutableList<string> Adjacents { get; private set; }

    [ExcludeFromCodeCoverage]
    public override string? ToString()
        => $"{Token} -> {StringExt.FromList(Adjacents)}";

    public Rule(string token, bool canStart, bool canEnd, IEnumerable<string> adjacents)
    {
        Token     = token;
        CanStart  = canStart;
        CanEnd    = canEnd;
        Adjacents = adjacents.ToImmutableList();
    }
}