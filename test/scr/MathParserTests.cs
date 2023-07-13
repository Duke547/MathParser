using NUnit.Framework;

namespace MathParsing.Testing;

internal class MathParserTests
{
    private static void TestValidExpression(string expression, decimal expectedResult)
    {
        Assert.Multiple(() =>
        {
            Assert.That(() => { var result = Parser.Parse(expression); }, Throws.Nothing, $"{expression}");
            Assert.That(Parser.Parse(expression), Is.EqualTo(expectedResult), $"{expression}");
        });
    }

    [Test]
    public void Parse_Valid_Tests()
    {
        Assert.Multiple(() =>
        {
            TestValidExpression("",                   0.0m);
            TestValidExpression("1 + 2 + 3",          6.0m);
            TestValidExpression("1 + 2 * 3",          7.0m);
            TestValidExpression("3 * 2 + 1",          7.0m);
            TestValidExpression("1+2+3",              6.0m);
            TestValidExpression("1 - 2 * 3",         -5.0m);
            TestValidExpression("2 × 3",              6.0m);
            TestValidExpression("2 ∙ 3",              6.0m);
            TestValidExpression("5 / 2",              2.5m);
            TestValidExpression("5 ÷ 2",              2.5m);
            TestValidExpression("5 % 4",              1.0m);
            TestValidExpression("3 * (2 + 1)",        9.0m);
            TestValidExpression("(3 * 2) + 1",        7.0m);
            TestValidExpression("(3 * 2) + (1 + 3)", 10.0m);
            TestValidExpression("()",                 0.0m);
            TestValidExpression("(5)",                5.0m);
            TestValidExpression("((5))",              5.0m);
        });
    }

    private static void TestInvalidExpression(string  expression,    Type expectedExceptionType, string expectedMessage,
                                              string? expectedToken)
    {
        if (expectedExceptionType == typeof(TokenException))
        {
            Assert.That(() => Parser.Parse(expression), Throws
                .TypeOf(expectedExceptionType)
                .With.Message             .EqualTo(expectedMessage)
                .And .Property("Token"   ).EqualTo(expectedToken), expression);
        }

        else
        {
            Assert.That(() => Parser.Parse(expression), Throws.TypeOf(expectedExceptionType)
                .With.Message.EqualTo(expectedMessage), expression);
        }
    }

    [Test]
    public void Parse_Invalid_Tests()
    {
        Assert.Multiple(() =>
        {
            TestInvalidExpression("1 + 1 &", typeof(TokenException),        "Unrecognized token '&'.", "&");
            TestInvalidExpression("1 + + 1", typeof(TokenException),        "Unexpected token '+'.",   "+");
            TestInvalidExpression("1 + 1 +", typeof(TokenException),        "Unexpected token '+'.",   "+");
            TestInvalidExpression("1 + 1 1", typeof(TokenException),        "Unexpected token '1'.",   "1");
            TestInvalidExpression("+ 1",     typeof(TokenException),        "Unexpected token '+'.",   "+");
            TestInvalidExpression("(1 + 1",  typeof(MissingTokenException), "Missing ')'.",            ")");
            TestInvalidExpression("1 + 1)",  typeof(MissingTokenException), "Missing '('.",            "(");
            TestInvalidExpression("(+ 1)",   typeof(TokenException),        "Unexpected token '+'.",   "+");
            TestInvalidExpression("(1 +)",   typeof(TokenException),        "Unexpected token ')'.",   ")");
        });
    }

    [Test]
    public void Parse_DivisionByZero_Test()
    {
        var expression = "1 / 0";

        Assert.That(() => Parser.Parse(expression), Throws.TypeOf<DivideByZeroException>());
    }
}
