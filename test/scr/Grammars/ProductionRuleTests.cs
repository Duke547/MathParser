using MathParsing.Grammars;
using NUnit.Framework;

namespace MathParsing.Testing.Grammars;

internal class ProductionRuleTests
{
    ProductionRule  _productionRule;
    GrammarSymbol[] _replacement;

    [SetUp]
    public void SetUp()
    {
        _replacement = new GrammarSymbol[]
        {
            new TerminalSymbol("B"),
            new TerminalSymbol("C")
        };

        _productionRule = new(new("A"), _replacement);
    }

    [Test]
    public void Nonterminal_Test()
    {
        Assert.That(_productionRule.Nonterminal, Is.EqualTo(new NonterminalSymbol("A")));
    }

    [Test]
    public void Replacement_Test()
    {
        Assert.That(_productionRule.Replacement, Is.EquivalentTo(_replacement));
    }
}