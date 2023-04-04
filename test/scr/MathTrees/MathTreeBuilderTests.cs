using MathParsing.MathTrees;
using NUnit.Framework;

namespace MathParsing.Testing.MathTrees;

internal class MathTreeBuilderTests
{
    [Test]
    public void Build_Test()
    {
        var tree = MathTreeBuilder.Build("1 + 2 + 3");

        Assert.That(tree.Value, Is.EqualTo(6));
    }
}