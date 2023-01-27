using MathParsing.Grammars;
using MathParsing.Lexing;
using MathParsing.MathTree;

namespace MathParsing;

public static class Parser
{
    static Grammar DefineGrammar()
    {
        var expression      = new GrammarSymbol("expression",       false);
        var binaryOperation = new GrammarSymbol("binary operation", false);
        var number          = new GrammarSymbol("number",           true );
        var addition        = new GrammarSymbol("addition",         true );

        var rules = new ProductionRule[]
        {
            new(expression,      new[] { number }),
            new(expression,      new[] { binaryOperation }),
            new(binaryOperation, new[] { expression, addition, expression })
        };

        return new(expression, rules);
    }

    public static decimal Parse(string expression)
    {
        var tokens             = Lexer.Tokenize(expression, TokenPatterns.All);
        var grammarTreeBuilder = new GrammarTreeBuilder(DefineGrammar());
        var grammarTree        = grammarTreeBuilder.Build(tokens);
        var mathTree           = MathTreeBuilder.Build(grammarTree);

        return mathTree.Value;
    }
}