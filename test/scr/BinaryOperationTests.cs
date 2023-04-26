using NUnit.Framework;

namespace MathParsing.Testing;

internal class BinaryOperationTests
{
    [Test]
    public void ToString_Test()
    {
        var binaryOperation = new BinaryOperation("+", (l, r) => 0, 0);

        Assert.That(binaryOperation.ToString(), Is.EqualTo("+"));
    }
}