using MathParsing.MathTrees;
using NUnit.Framework;

namespace MathParsing.Testing.MathTrees;

internal class GroupNodeTests
{
    GroupNode _node;

    [SetUp]
    public void SetUp()
    {
        _node = new GroupNode();
    }

    [Test]
    public void Value_Test()
    {
        var child = new NumberNode(5);
        
        _node.AddChild(child);

        Assert.That(_node.Value, Is.EqualTo(5));
    }

    [Test]
    public void Value_NoChild_Test()
    {
        Assert.That(_node.Value, Is.EqualTo(0));
    }

    [Test]
    public void AddChild_TryAddSecondChild_Test()
    {
        var parent = new GroupNode();
        var child1 = new GroupNode();
        var child2 = new GroupNode();

        parent.AddChild(child1);

        Assert.That(() => parent.AddChild(child2), Throws.InvalidOperationException);
    }
}