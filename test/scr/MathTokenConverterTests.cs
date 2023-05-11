using MathParsing.Grammars;
using MathParsing.Lexing;
using NUnit.Framework;

namespace MathParsing.Testing;

internal class MathTokenConverterTests
{
    [Test]
    public void Convert_InvalidToken_Test()
    {
        var token = new Token("unknown", "unknown", "a", 0);

        Assert.That(() => MathTokenConverter.Convert(token), Throws.TypeOf<TokenException>()
            .With.Message.EqualTo("Unrecognized unknown 'a'.")
            .And.Property("Token").EqualTo("a"));
    }

    [Test]
    public void Convert_InvalidNumberToken_Test()
    {
        var token = new Token("number", "number", "a", 0);

        Assert.That(() => MathTokenConverter.Convert(token), Throws.TypeOf<TokenException>()
            .With.Message.EqualTo("'a' does not represent a number.")
            .And.Property("Token").EqualTo("a"));
    }

    [Test]
    public void Convert_InvalidBinaryOperatorToken_Test()
    {
        var token = new Token("", "binary operator", "#", 0);

        Assert.That(() => MathTokenConverter.Convert(token), Throws.TypeOf<TokenException>()
            .With.Message.EqualTo("'#' does not represent a known binary operator.")
            .And.Property("Token").EqualTo("#"));
    }
}