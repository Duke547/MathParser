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
            "3 * (2 + 1)",
            "(3 * 2) + 1",
            "(3 * 2) + (1 + 3)",
            "()",
            "(5)",
            "((5))",
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
             1.0m,
             9.0m,
             7.0m,
            10.0m,
             0.0m,
             5.0m,
             5.0m,
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
    public void Parse_Invalid_Tests()
    {
        var expressions = new[]
        {
            "1 + 1 &",
            "1 + + 1",
            "1 + 1 +",
            "1 + 1 1",
            "+ 1",
            "(1 + 1",
            "1 + 1)",
            "(+ 1)",
            "(1 +)",
        };

        var expectedExceptionTypes = new[]
        {
            typeof(IndexedTokenException),
            typeof(IndexedTokenException),
            typeof(IndexedTokenException),
            typeof(IndexedTokenException),
            typeof(IndexedTokenException),
            typeof(InvalidOperationException),
            typeof(InvalidOperationException),
            typeof(IndexedTokenException),
            typeof(IndexedTokenException),
        };

        var expectedMessages = new[]
        {
            "Unrecognized token '&' at position 6.",
            "Unexpected token '+' at position 4.",
            "Unexpected token '+' at position 6.",
            "Unexpected token '1' at position 6.",
            "Unexpected token '+' at position 0.",
            "Missing ')'.",
            "Missing '('.",
            "Unexpected token '+' at position 1.",
            "Unexpected token ')' at position 4.",
        };

        var expectedTokens = new[]
        {
            "&",
            "+",
            "+",
            "1",
            "+",
            null,
            null,
            "+",
            ")",
        };

        var expectedPositions = new int?[]
        {
            6,
            4,
            6,
            6,
            0,
            null,
            null,
            1,
            4,
        };

        Assert.Multiple(() =>
        {
            for (int i = 0; i < expressions.Length; i++)
            {
                var expression    = expressions           [i];
                var exceptionType = expectedExceptionTypes[i];
                var message       = expectedMessages      [i];
                var token         = expectedTokens        [i];
                var position      = expectedPositions     [i];

                if (exceptionType == typeof(IndexedTokenException))
                {
                    Assert.That(() => Parser.Parse(expression), Throws.TypeOf<IndexedTokenException>()
                        .With.Message             .EqualTo(message)
                        .And .Property("Token"   ).EqualTo(token)
                        .And .Property("Position").EqualTo(position), expression);
                }
                else
                {
                    Assert.That(() => Parser.Parse(expression), Throws.TypeOf<InvalidOperationException>()
                        .With.Message.EqualTo(message), expression);
                }
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
