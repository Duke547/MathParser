using MathParsing.MathTrees;
using NUnit.Framework;

namespace MathParsing.Testing.MathTrees;

public class NumberNodeTests
{
    NumberNode _node;

    [SetUp]
    public void SetUp()
    {
        _node = new NumberNode(3);
    }

    [Test]
    public void Value_Test()
    {
        Assert.That(_node.Value, Is.EqualTo(3));
    }

    [Test]
    public void AddChild_TryAddChild_Test()
    {
        var child = new GroupNode();

        Assert.That(() => _node.AddChild(child), Throws.InvalidOperationException);
    }
}