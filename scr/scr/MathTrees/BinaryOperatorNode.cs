using MathParsing.Trees;

namespace MathParsing.MathTrees;

public sealed class BinaryOperatorNode : MathTreeNode
{
    new BinaryTreeNode<MathTreeNode> TreeNode => (base.TreeNode as BinaryTreeNode<MathTreeNode>)!;

    public string Symbol { get; private set; }

    public MathTreeNode? LeftChild => TreeNode.LeftChild?.Value;
    
    public MathTreeNode? RightChild => TreeNode.RightChild?.Value;

    public Func<decimal, decimal, decimal> Operation { get; private set; }

    public override decimal Value
    {
        get
        {
            if (LeftChild == null || RightChild == null)
                return 0;

            return Operation(LeftChild.Value, RightChild.Value);
        }
    }

    public BinaryOperatorNode(string symbol, Func<decimal, decimal, decimal> operation)
    {
        base.TreeNode = new BinaryTreeNode<MathTreeNode>(this);
        Symbol        = symbol;
        Operation     = operation;
    }
}