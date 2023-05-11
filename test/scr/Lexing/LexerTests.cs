using MathParsing.Grammars;
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
            new TokenPattern("number", "number",          @"\d"),
            new TokenPattern("add",    "binary operator", @"\+")
        };

        var expression = " 1 +2";
        var tokens     = Lexer.Tokenize(expression, patterns);

        var expectedTokens = new []
        {
            new Token("number", "number",          "1", 1),
            new Token("add",    "binary operator", "+", 3),
            new Token("number", "number",          "2", 4)
        };

        Assert.That(tokens, Is.EquivalentTo(expectedTokens));
    }

    [Test]
    public void Tokenize_UnrecongnizedPattern_Test()
    {
        var patterns = new []
        {
            new TokenPattern("number", "number", @"\d")
        };

        var expression      = "1 + 2";
        var expectedMessage = "Unrecognized token '+' at position 2.";

        Assert.That(() => Lexer.Tokenize(expression, patterns), Throws.TypeOf<IndexedTokenException>().With.Message.EqualTo(expectedMessage));
    }
}