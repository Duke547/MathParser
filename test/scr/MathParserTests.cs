using NUnit.Framework;

namespace MathParsing.Testing;

internal class MathParserTests
{
    [Test]
    public void Parse_Valid_Test()
    {
        var expressions = new string[]
        {
            "1 + 2 + 3",
            "1 + 2 * 3",
            "3 * 2 + 1",
            "1+2+3",
            "1 - 2 * 3",
            "2 × 3",
            "2 ∙ 3",
            "5 / 2",
            "5 ÷ 2",
            "5 % 4",
        };

        var expectedResults = new decimal[]
        {
             6.0m,
             7.0m,
             7.0m,
             6.0m,
            -5.0m,
             6.0m,
             6.0m,
             2.5m,
             2.5m,
             1.0m
        };

        Assert.Multiple(() =>
        {
            for (int i = 0; i < expressions.Length; i++)
            {
                var expression     = expressions[i];
                var expectedResult = expectedResults[i];
                
                Assert.That(() => { var result = Parser.Parse(expression); }, Throws.Nothing, $"{expression}");
                Assert.That(Parser.Parse(expression), Is.EqualTo(expectedResult), $"{expression}");
            }
        });
    }

    [Test]
    public void Parse_Invalid_Test()
    {
        var expression = "1 + 1 &";

        Assert.That(() => Parser.Parse(expression), Throws.ArgumentException.With.Message.EqualTo("Unrecognized symbol at '&'"));
    }

    [Test]
    public void Parse_DivisionByZero_Test()
    {
        var expression = "1 / 0";

        Assert.That(() => Parser.Parse(expression), Throws.TypeOf<DivideByZeroException>());
    }
}