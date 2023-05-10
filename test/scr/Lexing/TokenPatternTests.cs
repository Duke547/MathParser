using MathParsing.Lexing;
using NUnit.Framework;

namespace MathParsing.Testing.Lexing;

internal class TokenPatternTests
{
    [Test]
    public void Constructor_Test()
    {
        var description  = "A";
        var subset       = "B";
        var pattern      = "C";
        var tokenPattern = new TokenPattern(description, subset, pattern);

        Assert.Multiple(() =>
        {
            Assert.That(tokenPattern.Description, Is.EqualTo(description), "Descripton");
            Assert.That(tokenPattern.Subset,      Is.EqualTo(subset     ), "Descripton");
            Assert.That(tokenPattern.Pattern,     Is.EqualTo(pattern    ), "Pattern"   );
        });
    }
}