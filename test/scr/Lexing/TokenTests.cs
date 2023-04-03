using MathParsing.Lexing;
using NUnit.Framework;

namespace MathParsing.Testing.Lexing;

internal class TokenTests
{
    [Test]
    public void Description_Test()
    {
        var description = "D";
        var token       = new Token(description, "T");
        
        Assert.That(token.Description, Is.EqualTo(description));
    }

    [Test]
    public void Text_Test()
    {
        var text  = "T";
        var token = new Token("D", text);

        Assert.That(token.Text, Is.EqualTo(text));
    }

    [Test]
    public void Constructor_Test()
    {
        var text        = "T";
        var description = "D";
        var token       = new Token(description, text);

        Assert.Multiple(() =>
        {
            Assert.That(token.Description, Is.EqualTo(description), "token.Description");
            Assert.That(token.Text,        Is.EqualTo(text       ), "token.Text"       );
        });
    }
}