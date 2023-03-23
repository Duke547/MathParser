using MathParsing.Grammars;
using NUnit.Framework;

namespace MathParsing.Testing.Grammars;

public class GrammarTests
{
    Grammar                     _grammar;
    IEnumerable<ProductionRule> _rules = Array.Empty<ProductionRule>();

    Grammar DefineGrammar()
    {
        var expression      = new NonterminalSymbol("expression"      );
        var binaryOperation = new NonterminalSymbol("binary operation");
        var number          = new TerminalSymbol   ("number"          );
        var addition        = new TerminalSymbol   ("addition"        );

        _rules = new ProductionRule[]
        {
            new(expression,      new GrammarSymbol[] { number                           }),
            new(expression,      new GrammarSymbol[] { binaryOperation                  }),
            new(binaryOperation, new GrammarSymbol[] { expression, addition, expression })
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
        Assert.That(_grammar.Start, Is.EqualTo(new NonterminalSymbol("expression")));
    }

    [Test]
    public void Rules_Test()
    {
        Assert.That(_grammar.Rules, Is.EquivalentTo(_rules));
    }
}