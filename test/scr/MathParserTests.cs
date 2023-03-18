using NUnit.Framework;

namespace MathParsing.Testing;

public class MathParserTests
{
    [Test]
    public void Parse_Valid_Test()
    {
        var expression = "1 + 1 + 1";
        var expected   = 3m;

        Assert.That(Parser.Parse(expression), Is.EqualTo(expected));
    }

    [Test]
    public void Parse_Invalid_Test()
    {
        var expression = "1 + 1 &";

        Assert.That(() => Parser.Parse(expression), Throws.InvalidOperationException.With.Message.EqualTo("Unrecognized pattern at ^&"));
    }
}