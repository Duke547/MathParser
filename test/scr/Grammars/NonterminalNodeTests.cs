using MathParsing.Grammars;
using NUnit.Framework;

namespace MathParsing.Testing.Grammars;

public class NonterminalNodeTests
{
    NonterminalNode  _node;
    ProductionRule[] _nodeRules;

    [SetUp]
    public void SetUp()
    {
        _nodeRules = new ProductionRule[]
        {
            new(new NonterminalSymbol("B"), Array.Empty<GrammarSymbol>())
        };

        _node = new("A", _nodeRules);
    }

    [Test]
    public void Description_Test()
    {
        Assert.That(_node.Description, Is.EqualTo("A"));
    }

    [Test]
    public void Text_Test()
    {
        Assert.That(_node.Text, Is.EqualTo(""));
    }

    [Test]
    public void Rules_Test()
    {
        Assert.That(_node.Rules, Is.EquivalentTo(_nodeRules));
    }
}