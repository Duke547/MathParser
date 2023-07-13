using NUnit.Framework;

namespace MathParsing.Testing;

internal class UnaryOperationTests
{
    [Test]
    public void ToString_Test()
    {
        var unaryOperation = new UnaryOperation("+", o => o);

        Assert.That(unaryOperation.ToString(), Is.EqualTo("+"));
    }

    [Test]
    public void Constructor_Test()
    {
        var unaryOperation = new UnaryOperation("-", o => -o);

        Assert.Multiple(() =>
        {
            Assert.That(unaryOperation.Symbol,       Is.EqualTo("-"), "symbol"                  );
            Assert.That(unaryOperation.Operation(1), Is.EqualTo(-1),  "operation on the value 1");
        });
    }
}