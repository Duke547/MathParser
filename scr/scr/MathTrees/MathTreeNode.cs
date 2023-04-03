using System.Collections.Immutable;
using MathParsing.Trees;

namespace MathParsing.MathTrees;

internal abstract class MathTreeNode
{
    protected TreeNode<MathTreeNode> TreeNode { get; set; }

    public MathTreeNode? Parent => TreeNode
        .Parent?.Value;

    public ImmutableList<MathTreeNode> Children => TreeNode.Children
        .Select(c => c.Value)
        .ToImmutableList();

    public MathTreeNode Root => TreeNode.Root.Value;

    public abstract decimal Value { get; }

    public virtual void AddChild(MathTreeNode child)
        => TreeNode.AddChild(child.TreeNode);

    public MathTreeNode() =>
        TreeNode = new(this);
}