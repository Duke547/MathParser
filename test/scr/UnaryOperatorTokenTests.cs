using MathParsing.Lexing;
using NUnit.Framework;

namespace MathParsing.Testing;

internal class UnaryOperatorTokenTests
{
    [Test]
    public void ToString_Test()
    {
        var token = new UnaryOperatorToken(new("+", o => o));

        Assert.That(token.ToString(), Is.EqualTo("+"));
    }

    [Test]
    public void Constructor_Test()
    {
        var token = new UnaryOperatorToken(new("-", o => -o));

        Assert.Multiple(() =>
        {
            Assert.That(token.Symbol,                      Is.EqualTo("-"), "symbol"                  );
            Assert.That(token.UnaryOperation.Operation(1), Is.EqualTo(-1),  "operation on the value 1");
        });
    }
}