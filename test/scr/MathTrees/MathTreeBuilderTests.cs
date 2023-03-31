using MathParsing.Grammars;
using MathParsing.Lexing;
using MathParsing.MathTrees;
using NUnit.Framework;

namespace MathParsing.Testing.MathTrees;

public class MathTreeBuilderTests
{
    GrammarTreeNode _grammarTree;

    static Grammar DefineGrammar()
    {
        var expression      = new NonterminalSymbol("expression"      );
        var binaryOperation = new NonterminalSymbol("binary operation");
        var number          = new TerminalSymbol   ("number"          );
        var addition        = new TerminalSymbol   ("addition"        );

        var rules = new ProductionRule[]
        {
            new(binaryOperation, new GrammarSymbol[] { expression, addition, expression }),
            new(expression,      new GrammarSymbol[] { binaryOperation                  }),
            new(expression,      new GrammarSymbol[] { number                           })
        };

        return new(expression, rules);
    }

    [SetUp]
    public void SetUp()
    {
        var tokens = new Token[]
        {
            new("number",   "1"),
            new("addition", "+"),
            new("number",   "2"),
            new("addition", "+"),
            new("number",   "3")
        };

        _grammarTree = new GrammarTreeBuilder(DefineGrammar()).Build(tokens);
    }

    [Test]
    public void Build_Test()
    {
        var tree = MathTreeConverter.Build(_grammarTree);

        Assert.That(tree.Value, Is.EqualTo(6));
    }
}