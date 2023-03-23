using MathParsing.Grammars;
using NUnit.Framework;

namespace MathParsing.Testing.Grammars;

public class GrammarTests
{
    Grammar                     _grammar;
    IEnumerable<ProductionRule> _rules = Array.Empty<ProductionRule>();

    Grammar DefineGrammar()
    {
        var expression      = new GrammarSymbol("expression",       false);
        var binaryOperation = new GrammarSymbol("binary operation", false);
        var number          = new GrammarSymbol("number",           true );
        var addition        = new GrammarSymbol("addition",         true );

        _rules = new ProductionRule[]
        {
            new(expression,      new[] { number                           }),
            new(expression,      new[] { binaryOperation                  }),
            new(binaryOperation, new[] { expression, addition, expression })
        };

        return new(expression, _rules);
    }

    [SetUp]
    public void SetUp()
    {
        _grammar = DefineGrammar();
    }

    [Test]
    public void Start_Test()
    {
        Assert.That(_grammar.Start, Is.EqualTo(new GrammarSymbol("expression")));
    }

    [Test]
    public void Rules_Test()
    {
        Assert.That(_grammar.Rules, Is.EquivalentTo(_rules));
    }
}