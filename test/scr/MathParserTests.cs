using MathParsing.Grammars;
using NUnit.Framework;

namespace MathParsing.Testing;

internal class MathParserTests
{
    [Test]
    public void Parse_Valid_Tests()
    {
        var expressions = new string[]
        {
            "",
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
             0.0m,
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
    public void Parse_InvalidToken_Tests()
    {
        var expressions = new[]
        {
            "1 + 1 &",
            "1 + + 1",
            "1 + 1 +",
            "1 + 1 1",
            "+ 1"
        };

        var expectedMessages = new[]
        {
            "Unrecognized token '&' at position 6.",
            "Unexpected token '+' at position 4.",
            "Unexpected token '+' at position 6.",
            "Unexpected token '1' at position 6.",
            "Unexpected token '+' at position 0.",
        };

        var expectedTokens = new[]
        {
            "&",
            "+",
            "+",
            "1",
            "+",
        };

        var expectedPositions = new[]
        {
            6,
            4,
            6,
            6,
            0,
        };

        Assert.Multiple(() =>
        {
            for (int i = 0; i < expressions.Length; i++)
            {
                var expression    = expressions           [i];
                var message       = expectedMessages      [i];
                var token         = expectedTokens        [i];
                var position      = expectedPositions     [i];

                Assert.That(() => Parser.Parse(expression), Throws.TypeOf<IndexedTokenException>()
                    .With.Message             .EqualTo(message)
                    .And .Property("Token"   ).EqualTo(token)
                    .And .Property("Position").EqualTo(position), expression);
            }
        });
    }

    [Test]
    public void Parse_DivisionByZero_Test()
    {
        var expression = "1 / 0";

        Assert.That(() => Parser.Parse(expression), Throws.TypeOf<DivideByZeroException>());
    }
}