using MathParsing.Trees;
using NUnit.Framework;

namespace MathParsing.Testing.Trees;

internal class TreeNodeTests
{
    [Test]
    public void Value_Test()
    {
        var value = 1;
        var node  = new TreeNode<int>(value);

        Assert.That(node.Value, Is.EqualTo(value));
    }

    [Test]
    public void Parent_Test()
    {
        var parent = new TreeNode<int>(0);
        var child  = new TreeNode<int>(0);

        parent.AddChild(child);

        Assert.That(child.Parent, Is.EqualTo(parent));
    }

    [Test]
    public void Children_Test()
    {
        var parent = new TreeNode<int>(0);
        var child  = new TreeNode<int>(0);

        parent.AddChild(child);

        Assert.That(parent.Children, Does.Contain(child));
    }

    [Test]
    public void Root_Test()
    {
        var grandParent = new TreeNode<int>(0);
        var parent      = new TreeNode<int>(0);
        var child       = new TreeNode<int>(0);

        grandParent.AddChild(parent);
        parent.AddChild(child);

        Assert.That(child.Root, Is.EqualTo(grandParent));
    }

    [Test]
    public void AddChild_AddSelf_Test()
    {
        var child = new TreeNode<int>(0);

        Assert.That(() => child.AddChild(child), Throws.ArgumentException);
    }

    [Test]
    public void AddChild_ChildAlreadyHasParent_Test()
    {
        var parent1 = new TreeNode<int>(0);
        var parent2 = new TreeNode<int>(0);
        var child   = new TreeNode<int>(0);

        parent1.AddChild(child);

        Assert.That(() => parent2.AddChild(child), Throws.ArgumentException);
    }

    [Test]
    public void Remove_Test()
    {
        var parent = new TreeNode<string>("parent");
        var child  = new TreeNode<string>("child");

        parent.AddChild(child);
        child.Remove();
        
        Assert.Multiple(() =>
        {
            Assert.That(child.Parent,    Is.Null);
            Assert.That(parent.Children, Is.Empty);
        });
    }
}