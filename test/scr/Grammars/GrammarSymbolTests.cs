using MathParsing.Grammars;
using NUnit.Framework;

namespace MathParsing.Testing.Grammars;

public class GrammarSymbolTests
{
    GrammarSymbol _grammarSymbol;

    [SetUp]
    public void SetUp()
    {
        _grammarSymbol = new TerminalSymbol("D");
    }

    [Test]
    public void Description_Test()
    {
        Assert.That(_grammarSymbol.Description, Is.EqualTo("D"));
    }
}