using MathParsing.Grammars;
using MathParsing.Lexing;
using MathParsing.MathTrees;

namespace MathParsing;

public static class Parser
{
    static Grammar DefineGrammar()
    {
        var expression      = new NonterminalSymbol("expression"      );
        var binaryOperation = new NonterminalSymbol("binary operation");
        var number          = new TerminalSymbol   ("number"          );
        var addition        = new TerminalSymbol   ("addition"        );
        var multiplication  = new TerminalSymbol   ("multiplication"  );

        var rules = new ProductionRule[]
        {
            new(expression,      new GrammarSymbol[] { number }),
            new(expression,      new GrammarSymbol[] { binaryOperation }),
            new(binaryOperation, new GrammarSymbol[] { expression, addition, expression }),
            new(binaryOperation, new GrammarSymbol[] { expression, multiplication, expression })
        };

        return new(expression, rules);
    }

    public static decimal Parse(string expression)
    {
        var tokens             = Lexer.Tokenize(expression, TokenPatterns.All);
        var grammarTreeBuilder = new GrammarTreeBuilder(DefineGrammar());
        var grammarTree        = grammarTreeBuilder.Build(tokens);
        var mathTree           = MathTreeConverter.Build(grammarTree);

        return mathTree.Value;
    }
}