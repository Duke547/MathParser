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
    public void Validate_UndefinedToken_Test()
    {
        var tokens = new Token[]
        {
            new("D", "D", "D"),
        };

        Assert.That(() => _grammar.Validate(tokens), Throws.TypeOf<TokenException>()
            .With.Message          .EqualTo("Undefined token 'D'.")
            .And .Property("Token").EqualTo("D"));
    }

    [Test]
    public void Validate_UnexpectedTokens_Test()
    {
        var token1 = new Token("A", "A", "A");
        var token2 = new Token("B", "B", "B");
        var token3 = new Token("C", "C", "C");

        Assert.Multiple(() =>
        {
            Assert.That(() => _grammar.Validate(new[] { token2 }), Throws.TypeOf<TokenException>()
            .With.Message             .EqualTo("Unexpected token 'B'.")
            .And .Property("Token")   .EqualTo("B"));

            Assert.That(() => _grammar.Validate(new[] { token3 }), Throws.TypeOf<TokenException>()
            .With.Message             .EqualTo("Unexpected token 'C'.")
            .And .Property("Token")   .EqualTo("C"));

            Assert.That(() => _grammar.Validate(new[] { token2, token3, token3 }), Throws.TypeOf<TokenException>()
            .With.Message             .EqualTo("Unexpected token 'C'.")
            .And .Property("Token")   .EqualTo("C"));
        });
    }
}