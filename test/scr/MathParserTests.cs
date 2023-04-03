using NUnit.Framework;

namespace MathParsing.Testing;

internal class MathParserTests
{
    [Test]
    public void Parse_Valid_Test()
    {
        var expression = "1 + 2 * 3";
        var expected   = 7m;

        Assert.That(Parser.Parse(expression), Is.EqualTo(expected));
    }

    [Test]
    public void Parse_Invalid_Test()
    {
        var expression = "1 + 1 &";

        Assert.That(() => Parser.Parse(expression), Throws.ArgumentException.With.Message.EqualTo("Unrecognized symbol at '&'"));
    }
}