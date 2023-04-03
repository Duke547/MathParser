using MathParsing.Lexing;
using NUnit.Framework;

namespace MathParsing.Testing.Lexing;

internal class LexerTests
{
    [Test]
    public void Tokenize_Test()
    {
        var patterns = new []
        {
            new TokenPattern("number", @"\d"),
            new TokenPattern("add",    @"\+")
        };

        var expression = " 1 +2";
        var tokens     = Lexer.Tokenize(expression, patterns);

        var expectedTokens = new []
        {
            new Token("number", "1"),
            new Token("add",    "+"),
            new Token("number", "2")
        };

        Assert.That(tokens, Is.EqualTo(expectedTokens));
    }

    [Test]
    public void Tokenize_UnrecongnizedPattern_Test()
    {
        var patterns = new []
        {
            new TokenPattern("number", @"\d")
        };

        var expression      = "1 + 2";
        var expectedMessage = "Unrecognized symbol at '+ 2'";

        Assert.That(() => Lexer.Tokenize(expression, patterns), Throws.ArgumentException.With.Message.EqualTo(expectedMessage));
    }
}