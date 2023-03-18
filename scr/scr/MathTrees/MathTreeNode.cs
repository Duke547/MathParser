using System.Collections.Immutable;
using MathParsing.Trees;

namespace MathParsing.MathTrees;

public abstract class MathTreeNode
{
    readonly TreeNode<MathTreeNode> _treeNode;

    public MathTreeNode? Parent => _treeNode
        .Parent?.Value;

    public ImmutableList<MathTreeNode> Children => _treeNode.Children
        .Select(c => c.Value)
        .ToImmutableList();

    public MathTreeNode Root => _treeNode.Root.Value;

    public abstract decimal Value { get; }

    public abstract bool Open { get; }

    public virtual void AddChild(MathTreeNode child)
    {
        if (!Open)
            throw new InvalidOperationException("This node cannot have anymore children.");
        else
            _treeNode.AddChild(child._treeNode);
    }

    public MathTreeNode() =>
        _treeNode = new(this);
}