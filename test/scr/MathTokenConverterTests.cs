using MathParsing.Lexing;
using NUnit.Framework;

namespace MathParsing.Testing;

internal class MathTokenConverterTests
{
    [Test]
    public void Convert_InvalidNumberToken_Test()
    {
        var token = new Token("number", "a");

        Assert.That(() => MathTokenConverter.Convert(token),
            Throws.ArgumentException.With.Message.EqualTo("'a' does not represent a number."));
    }

    [Test]
    public void Convert_InvalidBinaryOperatorToken_Test()
    {
        var token = new Token("binary operator", "#");

        Assert.That(() => MathTokenConverter.Convert(token),
            Throws.ArgumentException.With.Message.EqualTo("'#' does not represent a known binary operator."));
    }
}