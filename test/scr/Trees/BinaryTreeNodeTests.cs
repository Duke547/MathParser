using MathParsing.Trees;
using NUnit.Framework;

namespace MathParsing.Testing.Trees;

internal class BinaryTreeNodeTests
{
    [Test]
    public void LeftChild_Test()
    {
        var parent = new BinaryTreeNode<string>("parent");
        var child  = new TreeNode      <string>("child" );

        Assert.Multiple(() =>
        {
            Assert.That(parent.LeftChild, Is.Null);

            parent.AddChild(child);

            Assert.That(parent.LeftChild, Is.EqualTo(child));
        });
    }

    [Test]
    public void RightChild_Test()
    {
        var parent = new BinaryTreeNode<string>("parent");
        var child1 = new TreeNode      <string>("child1");
        var child2 = new TreeNode      <string>("child2");

        Assert.Multiple(() =>
        {
            Assert.That(parent.RightChild, Is.Null);

            parent.AddChild(child1);
            parent.AddChild(child2);

            Assert.That(parent.RightChild, Is.EqualTo(child2));
        });
    }

    [Test]
    public void AddChild_TryAddThirdChild_Test()
    {
        var parent = new BinaryTreeNode<string>("parent");
        var child1 = new TreeNode      <string>("child1");
        var child2 = new TreeNode      <string>("child2");
        var child3 = new TreeNode      <string>("child3");

        parent.AddChild(child1);
        parent.AddChild(child2);

        Assert.That(() => parent.AddChild(child3), Throws.InvalidOperationException);
    }
}