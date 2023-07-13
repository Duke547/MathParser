using MathParsing.Lexing;
using NUnit.Framework;

namespace MathParsing.Testing;

internal class MathTokenConverterTests
{
    [Test]
    public void Convert_InvalidToken_Test()
    {
        var token = new Token("unknown", "unknown", "a");

        Assert.That(() => MathTokenConverter.Convert(token, null), Throws.TypeOf<TokenException>()
            .With.Message.EqualTo("Unrecognized unknown 'a'.")
            .And.Property("Token").EqualTo("a"));
    }

    [Test]
    public void Convert_InvalidNumberToken_Test()
    {
        var token = new Token("number", "number", "a");

        Assert.That(() => MathTokenConverter.Convert(token, null), Throws.TypeOf<TokenException>()
            .With.Message.EqualTo("'a' does not represent a number.")
            .And.Property("Token").EqualTo("a"));
    }

    [Test]
    public void Convert_UnaryOperators_Test()
    {
        var postiveToken      = new Token("plus",  "operator", "+");
        var negativeToken     = new Token("minus", "operator", "-");
        var positveMathToken  = (MathTokenConverter.Convert(postiveToken,  null) as UnaryOperatorToken)!;
        var negativeMathToken = (MathTokenConverter.Convert(negativeToken, null) as UnaryOperatorToken)!;

        Assert.Multiple(() =>
        {
            Assert.That(positveMathToken .Symbol,                      Is.EqualTo("+"), "postive math token symbol"                   );
            Assert.That(positveMathToken .UnaryOperation.Operation(1), Is.EqualTo(1),   "postive math token operation on the value 1" );
            Assert.That(negativeMathToken.Symbol,                      Is.EqualTo("-"), "negative math token symbol"                  );
            Assert.That(negativeMathToken.UnaryOperation.Operation(1), Is.EqualTo(-1),  "negative math token operation on the value 1");
        });
    }

    [Test]
    public void Convert_InvalidBinaryOperatorToken_Test()
    {
        var previousToken = new Token("number", "",         "2");
        var token         = new Token("",       "operator", "#");

        Assert.That(() => MathTokenConverter.Convert(token, previousToken), Throws.TypeOf<TokenException>()
            .With.Message.EqualTo("'#' does not represent a known binary operator.")
            .And.Property("Token").EqualTo("#"));
    }
}