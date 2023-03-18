namespace MathParsing.Trees;

public sealed class UnaryTreeNode<T> : TreeNode<T>
{
    public TreeNode<T>? Child => Children.ElementAtOrDefault(0);

    public override bool Open => Children.Count < 1;

    public UnaryTreeNode(T value) : base(value) { }
}