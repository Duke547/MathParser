namespace MathParsing.Trees;

internal sealed class LeafTreeNode<T> : TreeNode<T>
{
    public override bool IsOpen => false;

    public LeafTreeNode(T value) : base(value) { }
}