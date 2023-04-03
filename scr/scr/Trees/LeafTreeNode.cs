﻿namespace MathParsing.Trees;

internal sealed class LeafTreeNode<T> : TreeNode<T>
{
    public override bool Open => false;

    public LeafTreeNode(T value) : base(value) { }
}