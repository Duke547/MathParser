using MathParsing.Trees;

namespace MathParsing.MathTrees;

public sealed class NumberNode : MathTreeNode
{
    readonly decimal _value;

    public override decimal Value => _value;

    public NumberNode(decimal value)
    {
        _value = value;

        TreeNode = new LeafTreeNode<MathTreeNode>(this);
    }
}