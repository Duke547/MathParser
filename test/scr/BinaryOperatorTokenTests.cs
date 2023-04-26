using MathParsing.Lexing;
using NUnit.Framework;

namespace MathParsing.Testing;

internal class BinaryOperatorTokenTests
{
    [Test]
    public void ToString_Test()
    {
        var token = new BinaryOperatorToken(new("+", (l, r) => 0, 0));

        Assert.That(token.ToString(), Is.EqualTo("+"));
    }
}