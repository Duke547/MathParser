using MathParsing.Extensions;
using NUnit.Framework;

namespace MathParsing.Testing.Extensions;

internal class StringExtTests
{
    [Test]
    public void RemoveWhitespace_Test()
    {
        var input    = "A B\t C\n\r D";
        var expected = "ABCD";

        Assert.That(input.RemoveWhitespace(), Is.EqualTo(expected));
    }

    [Test]
    public void RemoveFirst_Tests()
    {
        Assert.Multiple(() =>
        {
            Assert.That("ABC".RemoveFirst("AB"),   Is.EqualTo("C"),   "Remove AB from ABC."  );
            Assert.That("ABC".RemoveFirst("AC"),   Is.EqualTo("ABC"), "Remove AC from ABC."  );
            Assert.That("ABC".RemoveFirst("C"),    Is.EqualTo("AB"),  "Remove C from ABC."   );
            Assert.That("ABC".RemoveFirst("ABCD"), Is.EqualTo("ABC"), "Remove ABCD from ABC.");
        });
    }

    [Test]
    public void FromList_Test()
    {
        var input    = new int[] { 1, 2, 3 };
        var expected = "1 2 3";

        Assert.That(StringExt.FromList(input), Is.EqualTo(expected));
    }
}