namespace MathParsing.Trees;

internal sealed class BinaryTreeNode<T> : TreeNode<T>
{
    public TreeNode<T>? LeftChild => Children.ElementAtOrDefault(0);

    public TreeNode<T>? RightChild => Children.ElementAtOrDefault(1);

    public override bool IsOpen => Children.Count < 2;

    public BinaryTreeNode(T value) : base(value) { }
}