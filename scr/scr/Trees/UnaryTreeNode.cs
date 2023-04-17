﻿namespace MathParsing.Trees;

internal sealed class UnaryTreeNode<T> : TreeNode<T>
{
    public TreeNode<T>? Child => Children.ElementAtOrDefault(0);

    public override bool IsOpen => Children.Count < 1;

    public UnaryTreeNode(T value) : base(value) { }
}