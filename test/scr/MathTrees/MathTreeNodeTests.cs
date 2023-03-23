using MathParsing.MathTrees;
using NUnit.Framework;

namespace MathParsing.Testing.MathTrees;

public class MathTreeNodeTests
{
    [Test]
    public void Parent_Test()
    {
        var parent = new GroupNode();
        var child  = new GroupNode();

        Assert.Multiple(() =>
        {
            Assert.That(child.Parent, Is.Null);

            parent.AddChild(child);

            Assert.That(child.Parent, Is.EqualTo(parent));
        });
    }

    [Test]
    public void Children_Test()
    {
        var parent = new GroupNode();
        var child  = new GroupNode();

        parent.AddChild(child);

        Assert.That(parent.Children, Does.Contain(child));
    }

    [Test]
    public void Root_Test()
    {
        var grandparent = new GroupNode();
        var parent      = new GroupNode();
        var child       = new GroupNode();

        grandparent.AddChild(parent);
        parent     .AddChild(child );

        Assert.That(child.Root, Is.EqualTo(grandparent));
    }

    [Test]
    public void AddChild_Test()
    {
        var parent = new GroupNode();
        var child  = new GroupNode();

        parent.AddChild(child);

        Assert.Multiple(() =>
        {
            Assert.That(child.Parent,    Is  .EqualTo(parent), "child.Parent"   );
            Assert.That(parent.Children, Does.Contain(child ), "parent.Children");
        });
    }
}