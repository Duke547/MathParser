﻿using System.Diagnostics.CodeAnalysis;

namespace MathParsing.Grammars;

internal abstract record GrammarSymbol
{
    public string Description { get; private set; }

    [ExcludeFromCodeCoverage]
    public override string? ToString() => Description;

    protected GrammarSymbol(string description)
        => Description = description;
}