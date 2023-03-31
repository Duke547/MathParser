using MathParsing.Grammars;

namespace MathParsing.MathTrees;

// TODO: Test invalid grammar tree nodes.
public static class MathTreeConverter
{
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

    // TODO: Rename to Convert
    public static MathTreeNode Build(GrammarTreeNode grammarTree)
    {
        var mathTree = ConvertToMathNode(grammarTree);

        foreach (var child in grammarTree.Children)
        {
            var branch = Build(child);
            
            mathTree.AddChild(branch);
        }

        return mathTree;
    }
}