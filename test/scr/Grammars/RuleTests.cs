using MathParsing.Grammars;
using NUnit.Framework;

namespace MathParsing.Testing.Grammars;

internal class RuleTests
{
    Rule  _grammarRule;

    [SetUp]
    public void SetUp()
    {
        _grammarRule = new("A", true, true , new[] { "A", "B" });
    }

    [Test]
    public void Token_Test()
    {
        Assert.That(_grammarRule.Token, Is.EqualTo("A"));
    }

    [Test]
    public void CanStart_Test()
    {
        Assert.That(_grammarRule.CanStart, Is.True);
    }

    [Test]
    public void CanEnd_Test()
    {
        Assert.That(_grammarRule.CanEnd, Is.True);
    }

    [Test]
    public void Adjacents_Test()
    {
        Assert.That(_grammarRule.Adjacents, Is.EquivalentTo(new [] { "A", "B" }));
    }
}