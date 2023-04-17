using MathParsing.Grammars;
using MathParsing.Lexing;
using NUnit.Framework;

namespace MathParsing.Testing.Grammars;

internal class GrammarTests
{
    Grammar _grammar;
    Rule[]  _rules;

    [SetUp]
    public void SetUp()
    {
        _rules = new Rule[]
        {
            new("A", false, false, new[] { "C"           }),
            new("B", true,  false, new[] { "A", "B", "C" }),
            new("C", false, true,  new[] { "B"           })
        };
        
        _grammar = new(_rules);
    }

    [Test]
    public void Construtor_MultipleRulesDefinedForSameToken_Test()
    {
        var rules = new Rule[]
        {
            new("A", false, false, new[] { "A" }),
            new("A", true,  false, new[] { "A" }),
        };

        Assert.That(() => new Grammar(rules), Throws.TypeOf<MultipleRulesForSameTokenException>()
            .With.Message.EqualTo("Multiple rules defined for token 'A'."));
    }

    [Test]
    public void Rules_Test()
    {
        Assert.That(_grammar.Rules, Is.EquivalentTo(_rules));
    }

    [Test]
    public void Validate_Test()
    {
        var tokens = new Token[]
        {
            new("B", "B"),
            new("A", "A"),
            new("C", "C")
        };
        
        Assert.That(() => _grammar.Validate(tokens), Throws.Nothing);
    }

    [Test]
    public void Validate_UnDefinedToken_Test()
    {
        var tokens = new Token[]
        {
            new("D", "D"),
        };

        Assert.That(() => _grammar.Validate(tokens), Throws.TypeOf<UndefinedTokenException>()
            .With.Message.EqualTo("Undefined token 'D'."));
    }

    [Test]
    public void Validate_UnexpectedTokens_Test()
    {
        var token1 = new Token("A", "A");
        var token2 = new Token("B", "B");
        var token3 = new Token("C", "C");

        Assert.Multiple(() =>
        {
            Assert.That(() => _grammar.Validate(new[] { token2 }), Throws.TypeOf<UnexpectedTokenException>()
                .With.Message.EqualTo("Unexpected token 'B'."));

            Assert.That(() => _grammar.Validate(new[] { token3 }), Throws.TypeOf<UnexpectedTokenException>()
                .With.Message.EqualTo("Unexpected token 'C'."));

            Assert.That(() => _grammar.Validate(new[] { token2, token3, token3 }), Throws.TypeOf<UnexpectedTokenException>()
                .With.Message.EqualTo("Unexpected token 'C'."));
        });
    }
}