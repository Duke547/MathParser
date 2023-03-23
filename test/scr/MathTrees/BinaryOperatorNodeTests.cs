using MathParsing.MathTrees;
using MathParsing.Trees;
using NUnit.Framework;

namespace MathParsing.Testing.MathTrees;

public class BinaryOperatorNodeTests
{
    readonly Func<decimal, decimal, decimal> _operation = (l, r) => l + r;
    
    BinaryOperatorNode _node;

    [SetUp]
    public void SetUp()
    {
        _node = new BinaryOperatorNode("+", _operation);
    }

    [Test]
    public void Symbol_Test()
    {
        Assert.That(_node.Symbol, Is.EqualTo("+"));
    }

    [Test]
    public void Operation_Test()
    {
        Assert.That(_node.Operation, Is.EqualTo(_operation));
    }

    [Test]
    public void Value_Test()
    {
        var child1 = new NumberNode(5);
        var child2 = new NumberNode(6);

        _node.AddChild(child1);
        _node.AddChild(child2);

        Assert.That(_node.Value, Is.EqualTo(11));
    }

    [Test]
    public void Value_LeftChildIsNull_Test()
    {
        Assert.That(_node.Value, Is.EqualTo(0));
    }

    [Test]
    public void Value_RightChildIsNull_Test()
    {
        var child = new NumberNode(5);

        _node.AddChild(child);

        Assert.That(_node.Value, Is.EqualTo(0));
    }

    [Test]
    public void LeftChild_Test()
    {
        var child = new NumberNode(0);

        _node.AddChild(child);

        Assert.That(_node.LeftChild, Is.EqualTo(child));
    }

    [Test]
    public void RightChild_Test()
    {
        var child1 = new NumberNode(5);
        var child2 = new NumberNode(6);

        _node.AddChild(child1);
        _node.AddChild(child2);

        Assert.That(_node.LeftChild, Is.EqualTo(child1));
    }

    [Test]
    public void AddChild_TryAddThirdChild_Test()
    {
        var child1 = new NumberNode(0);
        var child2 = new NumberNode(0);
        var child3 = new NumberNode(0);

        _node.AddChild(child1);
        _node.AddChild(child2);

        Assert.That(() => _node.AddChild(child3), Throws.InvalidOperationException);
    }
}