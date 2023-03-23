using MathParsing.Trees;

namespace MathParsing.MathTrees;

public sealed class GroupNode : MathTreeNode
{
    new UnaryTreeNode<MathTreeNode> TreeNode => (base.TreeNode as UnaryTreeNode<MathTreeNode>)!;

    public override decimal Value
        => TreeNode.Child?.Value != null ? TreeNode.Child.Value.Value : 0;

    public GroupNode()
        => base.TreeNode = new UnaryTreeNode<MathTreeNode>(this);
}