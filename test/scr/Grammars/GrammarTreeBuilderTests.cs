using MathParsing.Grammars;
using MathParsing.Lexing;
using NUnit.Framework;

namespace MathParsing.Testing.Grammars;

internal class GrammarTreeBuilderTests
{
    GrammarTreeBuilder _builder;

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
        _builder = new(DefineGrammar());
    }

    [Test]
    public void Build_Test()
    {
        var tokens = new Token[]
        {
            new("number",   "1"),
            new("addition", "+"),
            new("number",   "2"),
            new("addition", "+"),
            new("number",   "3")
        };

        var tree = _builder.Build(tokens);

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

        var tokens = new Token[]
        {
            new("number",   "1"),
            new("addition", "+"),
            new("number",   "2"),
            new("addition", "+"),
            new("number",   "3")
        };

        var tree = _builder.Build(tokens);

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

        Assert.That(() => _builder.Build(tokens), Throws.ArgumentException.With.Message.EqualTo("Unexpected token '+'."));
    }

    [Test]
    public void Build_UnknownToken_Test()
    {
        var tokens = new Token[]
        {
            new("number",      "1"),
            new("subtraction", "-"),
            new("number",      "2"),
        };

        Assert.That(() => _builder.Build(tokens), Throws.ArgumentException.With.Message.EqualTo("Unexpected token '-'."));
    }

    [Test]
    public void Build_NoTokens_Test()
    {
        var tokens = Array.Empty<Token>();

        Assert.That(() => _builder.Build(tokens), Throws.Nothing);
    }

    [Test]
    public void Build_NoRules_Test()
    {
        var tokens = new Token[]
        {
            new("number",      "1"),
            new("subtraction", "-"),
            new("number",      "2"),
        };

        _builder.Grammar.Rules.Clear();

        Assert.That(() => _builder.Build(tokens), Throws.InvalidOperationException
            .With.Message.EqualTo("The associated grammar does not define any production rules."));
    }
}