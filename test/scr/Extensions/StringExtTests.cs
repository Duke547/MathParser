using MathParsing.Extensions;
using NUnit.Framework;

namespace MathParsing.Testing.Extensions;

public class StringExtTests
{
    [Test]
    public void RemoveWhitespace_Test()
    {
        var input    = "A B\t C\n\r D";
        var expected = "ABCD";

        Assert.That(input.RemoveWhitespace(), Is.EqualTo(expected));
    }

    [Test]
    public void FromList_Test()
    {
        var input    = new int[] { 1, 2, 3 };
        var expected = "1 2 3";

        Assert.That(StringExt.FromList(input), Is.EqualTo(expected));
    }
}