using MathParsing.Grammars;

namespace MathParsing.MathTree;

public sealed class MathTreeBuilder
{
    static NumberNode ConvertToNumberNode(GrammarTreeNode grammarNode)
    {
        var number = decimal.Parse(grammarNode.Text);

        return new(number);
    }

    static Func<decimal, decimal, decimal> ConvertToBinaryOperation(string description)
        => (l, r) => l + r;

    static BinaryOperatorNode ConvertToBinaryOperatorNode(GrammarTreeNode grammarNode)
    {
        var operatorChild = grammarNode.Children[1];
        var symbol        = operatorChild.Text;
        var operation     = ConvertToBinaryOperation(operatorChild.Symbol.Description);

        operatorChild.Remove();

        return new(symbol, operation);
    }

    public static MathTreeNode ConvertToMathNode(GrammarTreeNode grammarTree)
    {
        var description = grammarTree.Symbol.Description;

        if (description == "number")
            return ConvertToNumberNode(grammarTree);

        else if (description == "binary operation")
            return ConvertToBinaryOperatorNode(grammarTree);

        else
            return new GroupNode();
    }

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