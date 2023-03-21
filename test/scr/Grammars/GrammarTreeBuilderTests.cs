using MathParsing.Grammars;
using MathParsing.Lexing;
using NUnit.Framework;

namespace MathParsing.Testing.Grammars;

public class GrammarTreeBuilderTests
{
    GrammarTreeBuilder _builder;

    static Grammar DefineGrammar()
    {
        var expression      = new GrammarSymbol("expression",       false);
        var binaryOperation = new GrammarSymbol("binary operation", false);
        var number          = new GrammarSymbol("number",           true );
        var addition        = new GrammarSymbol("addition",         true );

        var rules = new ProductionRule[]
        {
            new(expression,      new[] { number                           }),
            new(expression,      new[] { binaryOperation                  }),
            new(binaryOperation, new[] { expression, addition, expression })
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
            Assert.That(tree                                                            .Symbol.Description, Is.EqualTo("expression"      ));
            Assert.That(tree.Children[0]                                                .Symbol.Description, Is.EqualTo("binary operation"));
            Assert.That(tree.Children[0].Children[0]                                    .Symbol.Description, Is.EqualTo("expression"      ));
            Assert.That(tree.Children[0].Children[0].Children[0]                        .Symbol.Description, Is.EqualTo("number"          ));
            Assert.That(tree.Children[0].Children[0].Children[0]                        .Text,               Is.EqualTo("1"               ));
            Assert.That(tree.Children[0].Children[1]                                    .Symbol.Description, Is.EqualTo("addition"        ));
            Assert.That(tree.Children[0].Children[2]                                    .Symbol.Description, Is.EqualTo("expression"      ));
            Assert.That(tree.Children[0].Children[2].Children[0]                        .Symbol.Description, Is.EqualTo("binary operation"));
            Assert.That(tree.Children[0].Children[2].Children[0].Children[0]            .Symbol.Description, Is.EqualTo("expression"      ));
            Assert.That(tree.Children[0].Children[2].Children[0].Children[0].Children[0].Symbol.Description, Is.EqualTo("number"          ));
            Assert.That(tree.Children[0].Children[2].Children[0].Children[0].Children[0].Text,               Is.EqualTo("2"               ));
            Assert.That(tree.Children[0].Children[2].Children[0].Children[1]            .Symbol.Description, Is.EqualTo("addition"        ));
            Assert.That(tree.Children[0].Children[2].Children[0].Children[2]            .Symbol.Description, Is.EqualTo("expression"      ));
            Assert.That(tree.Children[0].Children[2].Children[0].Children[2].Children[0].Symbol.Description, Is.EqualTo("number"          ));
            Assert.That(tree.Children[0].Children[2].Children[0].Children[2].Children[0].Text,               Is.EqualTo("3"               ));
        });
    }

    [Test]
    public void Build_Invalid_ExtraToken_Test()
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
    public void Build_Invalid_UnknownToken_Test()
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
    public void Build_Invalid_Empty_Test()
    {
        var tokens = Array.Empty<Token>();

        Assert.That(() => _builder.Build(tokens), Throws.Nothing);
    }
}