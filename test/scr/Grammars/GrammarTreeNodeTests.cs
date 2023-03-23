using MathParsing.Grammars;
using NUnit.Framework;

namespace MathParsing.Testing.Grammars;

public class GrammarTreeNodeTests
{
    GrammarTreeNode _treeNode;

    [SetUp]
    public void SetUp()
    {
        _treeNode = new GrammarTreeNode(new TerminalSymbol("A"), Array.Empty<ProductionRule>());
    }

    [Test]
    public void Symbol_Test()
    {
        Assert.That(_treeNode.Symbol, Is.EqualTo(new TerminalSymbol("A")));
    }

    [Test]
    public void Text_Test()
    {
        Assert.Multiple(() =>
        {
            Assert.That(_treeNode.Text, Is.EqualTo("A"));

            _treeNode.Token = new("B", "C");

            Assert.That(_treeNode.Text, Is.EqualTo("C"));
        });
    }

    [Test]
    public void Parent_Test()
    {
        Assert.Multiple(() =>
        {
            Assert.That(_treeNode.Parent, Is.Null);

            var parent = new GrammarTreeNode(new TerminalSymbol("B"), Array.Empty<ProductionRule>());

            parent.AddChild(_treeNode);

            Assert.That(_treeNode.Parent, Is.EqualTo(parent));
        });
    }

    [Test]
    public void Children_Test()
    {
        var child = new GrammarTreeNode(new TerminalSymbol("B"), Array.Empty<ProductionRule>());

        _treeNode.AddChild(child);

        Assert.That(_treeNode.Children, Does.Contain(child));
    }

    [Test]
    public void Remove_Test()
    {
        var child = new GrammarTreeNode(new TerminalSymbol("B"), Array.Empty<ProductionRule>());

        _treeNode.AddChild(child);

        child.Remove();

        Assert.That(_treeNode.Children, Does.Not.Contain(child));
    }
}