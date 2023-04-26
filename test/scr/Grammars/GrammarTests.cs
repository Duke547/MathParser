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
            new("D", "D", 0),
        };

        Assert.That(() => _grammar.Validate(tokens), Throws.TypeOf<IndexedTokenException>()
            .With.Message             .EqualTo("Undefined token 'D' at position 0.")
            .And .Property("Token")   .EqualTo("D")
            .And .Property("Position").EqualTo(0));
    }

    [Test]
    public void Validate_UnexpectedTokens_Test()
    {
        var token1 = new Token("A", "A", 0);
        var token2 = new Token("B", "B", 1);
        var token3 = new Token("C", "C", 2);

        Assert.Multiple(() =>
        {
            Assert.That(() => _grammar.Validate(new[] { token2 }), Throws.TypeOf<IndexedTokenException>()
            .With.Message             .EqualTo("Unexpected token 'B' at position 1.")
            .And .Property("Token")   .EqualTo("B")
            .And .Property("Position").EqualTo(1));

            Assert.That(() => _grammar.Validate(new[] { token3 }), Throws.TypeOf<IndexedTokenException>()
            .With.Message             .EqualTo("Unexpected token 'C' at position 2.")
            .And .Property("Token")   .EqualTo("C")
            .And .Property("Position").EqualTo(2));

            Assert.That(() => _grammar.Validate(new[] { token2, token3, token3 }), Throws.TypeOf<IndexedTokenException>()
            .With.Message             .EqualTo("Unexpected token 'C' at position 2.")
            .And .Property("Token")   .EqualTo("C")
            .And .Property("Position").EqualTo(2));
        });
    }
}