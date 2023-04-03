using MathParsing.Grammars;
using MathParsing.Lexing;
using NUnit.Framework;

namespace MathParsing.Testing.Grammars;

internal class TerminalNodeTests
{
    TerminalNode _node;

    [SetUp]
    public void SetUp()
    {
        _node = new(new("A", "B"));
    }

    [Test]
    public void Token_Test()
    {
        var token = new Token("A", "B");

        Assert.That(_node.Token, Is.EqualTo(token));
    }

    [Test]
    public void Description_Test()
    {
        Assert.That(_node.Description, Is.EqualTo("A"));
    }

    [Test]
    public void Text_Test()
    {
        Assert.That(_node.Text, Is.EqualTo("B"));
    }

    [Test]
    public void AddChild_Test()
    {
        var child = new TerminalNode(new("C", "D"));

        Assert.That(() => _node.AddChild(child), Throws.InvalidOperationException);
    }
}