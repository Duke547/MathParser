using NUnit.Framework;

namespace MathParsing.Testing;

internal class BracketTokenTests
{
    [Test]
    public void ToString_Test()
    {
        var leftBracket  = new BracketToken(true);
        var rightBracket = new BracketToken(false);

        Assert.Multiple(() =>
        {
            Assert.That(leftBracket .ToString(), Is.EqualTo("("));
            Assert.That(rightBracket.ToString(), Is.EqualTo(")"));
        });
    }
}