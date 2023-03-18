using MathParsing.Lexing;
using NUnit.Framework;

namespace MathParsing.Testing.Lexing;

public class TokenPatternTests
{
    [Test]
    public void Description_Test()
    {
        var description  = "D";
        var tokenPattern = new TokenPattern(description, "P");
        
        Assert.That(tokenPattern.Description, Is.EqualTo(description));
    }

    [Test]
    public void Pattern_Test()
    {
        var pattern      = "P";
        var tokenPattern = new TokenPattern("D", pattern);

        Assert.That(tokenPattern.Pattern, Is.EqualTo(pattern));
    }

    [Test]
    public void Constructor_Test()
    {
        var pattern      = "P";
        var description  = "D";
        var tokenPattern = new TokenPattern(description, pattern);

        Assert.Multiple(() =>
        {
            Assert.That(tokenPattern.Description, Is.EqualTo(description), "Descripton");
            Assert.That(tokenPattern.Pattern,     Is.EqualTo(pattern    ), "Pattern"   );
        });
    }
}