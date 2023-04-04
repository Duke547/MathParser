using MathParsing.Grammars;
using MathParsing.Lexing;

namespace MathParsing.MathTrees;

internal static class MathTreeBuilder
{
    private static Grammar DefineGrammar()
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

    private static TokenPattern[] DefinePatterns()
    {
        return new TokenPattern[]
        {
            new("number",         @"\d*\.?\d+"),
            new("addition",       @"\+"       ),
            new("multiplication", @"\*"       )
        };
    }

    static NumberNode ConvertToNumberNode(TerminalNode terminalNode)
    {
        var number = decimal.Parse(terminalNode.Token.Text);

        return new(number);
    }

    static Func<decimal, decimal, decimal> ConvertToBinaryOperation(string description) => description switch
    {
        "addition" => (l, r) => l + r,
        _          => (l, r) => l * r,
    };

    static BinaryOperatorNode ConvertToBinaryOperatorNode(NonterminalNode nonterminalNode)
    {
        var operatorChild = (nonterminalNode.Children[1] as TerminalNode)!;
        var symbol        = operatorChild.Token.Text;
        var operation     = ConvertToBinaryOperation(operatorChild.Description);

        operatorChild.Remove();

        return new(symbol, operation);
    }

    static MathTreeNode ConvertToMathNode(GrammarTreeNode tree)
    {
        if (tree is TerminalNode terminalNode)
        {
            if (terminalNode.Description == "number")
                return ConvertToNumberNode(terminalNode);
        }
        else if (tree is NonterminalNode nonterminalNode)
        {
            if (nonterminalNode.Description == "binary operation")
                return ConvertToBinaryOperatorNode(nonterminalNode);
        }

        return new GroupNode();
    }

    private static MathTreeNode Build(GrammarTreeNode grammarTree)
    {
        var mathTree = ConvertToMathNode(grammarTree);

        foreach (var child in grammarTree.Children)
        {
            var branch = Build(child);

            mathTree.AddChild(branch);
        }

        return mathTree;
    }

    public static MathTreeNode Build(string expression)
    {
        var grammarTreeBuilder = new GrammarTreeBuilder(DefineGrammar(), DefinePatterns());
        var grammarTree        = grammarTreeBuilder.Build(expression);
        
        return Build(grammarTree);
    }
}