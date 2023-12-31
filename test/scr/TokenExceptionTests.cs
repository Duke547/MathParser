﻿using NUnit.Framework;

namespace MathParsing.Testing;

internal class TokenExceptionTests
{
    [Test]
    public void Constructor_Test()
    {
        var exception = new TokenException("A", "B");

        Assert.Multiple(() =>
        {
            Assert.That(exception.Token,   Is.EqualTo("A"));
            Assert.That(exception.Message, Is.EqualTo("B"));
        });
    }
}