using MathParsing.Grammars;
using NUnit.Framework;

namespace MathParsing.Testing.Grammars;

internal class GrammarTreeNodeTests
{
    GrammarTreeNode _treeNode;

    [SetUp]
    public void SetUp()
    {
        _treeNode = new NonterminalNode("A", Array.Empty<ProductionRule>());
    }

    [Test]
    public void Parent_Test()
    {
        Assert.Multiple(() =>
        {
            Assert.That(_treeNode.Parent, Is.Null);

            var parent = new NonterminalNode("B", Array.Empty<ProductionRule>());

            parent.AddChild(_treeNode);

            Assert.That(_treeNode.Parent, Is.EqualTo(parent));
        });
    }

    [Test]
    public void Children_Test()
    {
        var child = new NonterminalNode("B", Array.Empty<ProductionRule>());

        _treeNode.AddChild(child);

        Assert.That(_treeNode.Children, Does.Contain(child));
    }

    [Test]
    public void Remove_Test()
    {
        var child = new NonterminalNode("B", Array.Empty<ProductionRule>());

        _treeNode.AddChild(child);

        child.Remove();

        Assert.That(_treeNode.Children, Does.Not.Contain(child));
    }
}