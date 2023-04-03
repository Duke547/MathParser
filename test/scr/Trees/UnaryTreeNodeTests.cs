using MathParsing.Trees;
using NUnit.Framework;

namespace MathParsing.Testing.Trees;

internal class UnaryTreeNodeTests
{
    [Test]
    public void Child_Test()
    {
        var parent = new UnaryTreeNode<string>("parent");
        var child  = new TreeNode     <string>("child" );

        Assert.Multiple(() =>
        {
            Assert.That(parent.Child, Is.Null);

            parent.AddChild(child);

            Assert.That(parent.Child, Is.EqualTo(child));
        });
    }

    [Test]
    public void AddChild_TryAddSecondChild_Test()
    {
        var parent = new UnaryTreeNode<string>("parent");
        var child1 = new TreeNode     <string>("child1");
        var child2 = new TreeNode     <string>("child2");

        parent.AddChild(child1);

        Assert.That(() => parent.AddChild(child2), Throws.InvalidOperationException);
    }
}