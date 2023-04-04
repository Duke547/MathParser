using MathParsing.Grammars;
using MathParsing.Lexing;
using NUnit.Framework;

namespace MathParsing.Testing.Grammars;

internal class GrammarTreeBuilderTests
{
    GrammarTreeBuilder _builder;
    Grammar            _grammar;

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

    static TokenPattern[] DefinePatterns()
    {
        return new TokenPattern[]
        {
            new("number",   @"\d*\.?\d+"),
            new("addition", @"\+"       ),
        };
    }

    [SetUp]
    public void SetUp()
    {
        _grammar = DefineGrammar();
        _builder = new(_grammar, DefinePatterns());
    }

    [Test]
    public void Constructor_Test()
    {
        Assert.Multiple(() =>
        {
            Assert.That(_builder.Grammar,       Is.EqualTo(_grammar));
            Assert.That(_builder.TokenPatterns, Is.EqualTo(DefinePatterns()));
        });
    }

    [Test]
    public void Build_Test()
    {
        var tree = _builder.Build("1 + 2 + 3");

        Assert.Multiple(() =>
        {
            Assert.That(tree                                                            .Description, Is.EqualTo("expression"      ));
            Assert.That(tree.Children[0]                                                .Description, Is.EqualTo("binary operation"));
            Assert.That(tree.Children[0].Children[0]                                    .Description, Is.EqualTo("expression"      ));
            Assert.That(tree.Children[0].Children[0].Children[0]                        .Description, Is.EqualTo("number"          ));
            Assert.That(tree.Children[0].Children[0].Children[0]                        .Text,        Is.EqualTo("1"               ));
            Assert.That(tree.Children[0].Children[1]                                    .Description, Is.EqualTo("addition"        ));
            Assert.That(tree.Children[0].Children[2]                                    .Description, Is.EqualTo("expression"      ));
            Assert.That(tree.Children[0].Children[2].Children[0]                        .Description, Is.EqualTo("binary operation"));
            Assert.That(tree.Children[0].Children[2].Children[0].Children[0]            .Description, Is.EqualTo("expression"      ));
            Assert.That(tree.Children[0].Children[2].Children[0].Children[0].Children[0].Description, Is.EqualTo("number"          ));
            Assert.That(tree.Children[0].Children[2].Children[0].Children[0].Children[0].Text,        Is.EqualTo("2"               ));
            Assert.That(tree.Children[0].Children[2].Children[0].Children[1]            .Description, Is.EqualTo("addition"        ));
            Assert.That(tree.Children[0].Children[2].Children[0].Children[2]            .Description, Is.EqualTo("expression"      ));
            Assert.That(tree.Children[0].Children[2].Children[0].Children[2].Children[0].Description, Is.EqualTo("number"          ));
            Assert.That(tree.Children[0].Children[2].Children[0].Children[2].Children[0].Text,        Is.EqualTo("3"               ));
        });
    }

    [Test]
    public void Build_ReverseGrammarRules_Test()
    {
        _builder.Grammar.Rules.Reverse();

        var tree = _builder.Build("1 + 2 + 3");

        Assert.Multiple(() =>
        {
            Assert.That(tree                                                            .Description, Is.EqualTo("expression"      ));
            Assert.That(tree.Children[0]                                                .Description, Is.EqualTo("binary operation"));
            Assert.That(tree.Children[0].Children[0]                                    .Description, Is.EqualTo("expression"      ));
            Assert.That(tree.Children[0].Children[0].Children[0]                        .Description, Is.EqualTo("number"          ));
            Assert.That(tree.Children[0].Children[0].Children[0]                        .Text,        Is.EqualTo("1"               ));
            Assert.That(tree.Children[0].Children[1]                                    .Description, Is.EqualTo("addition"        ));
            Assert.That(tree.Children[0].Children[2]                                    .Description, Is.EqualTo("expression"      ));
            Assert.That(tree.Children[0].Children[2].Children[0]                        .Description, Is.EqualTo("binary operation"));
            Assert.That(tree.Children[0].Children[2].Children[0].Children[0]            .Description, Is.EqualTo("expression"      ));
            Assert.That(tree.Children[0].Children[2].Children[0].Children[0].Children[0].Description, Is.EqualTo("number"          ));
            Assert.That(tree.Children[0].Children[2].Children[0].Children[0].Children[0].Text,        Is.EqualTo("2"               ));
            Assert.That(tree.Children[0].Children[2].Children[0].Children[1]            .Description, Is.EqualTo("addition"        ));
            Assert.That(tree.Children[0].Children[2].Children[0].Children[2]            .Description, Is.EqualTo("expression"      ));
            Assert.That(tree.Children[0].Children[2].Children[0].Children[2].Children[0].Description, Is.EqualTo("number"          ));
            Assert.That(tree.Children[0].Children[2].Children[0].Children[2].Children[0].Text,        Is.EqualTo("3"               ));
        });
    }

    [Test]
    public void Build_ExtraToken_Test()
    {
        var tokens = new Token[]
        {
            new("number",   "1"),
            new("addition", "+"),
            new("number",   "2"),
            new("addition", "+")
        };

        Assert.That(() => _builder.Build("1 + 2 +"), Throws.ArgumentException.With.Message.EqualTo("Unexpected token '+'."));
    }

    [Test]
    public void Build_UnknownToken_Test()
    {
        Assert.That(() => _builder.Build("1 - 2"), Throws.ArgumentException.With.Message.EqualTo("Unrecognized symbol at '- 2'"));
    }

    [Test]
    public void Build_EmptyExpression_Test()
    {
        Assert.That(() => _builder.Build(""), Throws.Nothing);
    }

    [Test]
    public void Build_NoRules_Test()
    {
        _builder.Grammar.Rules.Clear();

        Assert.That(() => _builder.Build("1 + 2"), Throws.InvalidOperationException
            .With.Message.EqualTo("The associated grammar does not define any production rules."));
    }
}