using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;

namespace MathParsing.Trees;

public class TreeNode<T>
{
    readonly List<TreeNode<T>> _children = new();

    public T Value { get; private set; }

    public TreeNode<T>? Parent { get; private set; }

    public ImmutableList<TreeNode<T>> Children => _children.ToImmutableList();

    public TreeNode<T> Root
    {
        get
        {
            if (Parent != null)
                return Parent.Root;
            else
                return this;
        }
    }

    public virtual bool Open => true;

    public void AddChild(TreeNode<T> child)
    {
        if (child == this)
            throw new ArgumentException("Cannot add self as child.", nameof(child));

        if (child.Parent != null)
            throw new ArgumentException("child already has a parent.", nameof(child));

        if (!Open)
            throw new InvalidOperationException($"This tree node cannot not have more than {Children.Count} children.");

        _children.Add(child);
        child.Parent = this;
    }

    public void Remove()
    {
        if (Parent is not null)
        {
            Parent._children.Remove(this);
            Parent = null;
        }
    }

    [ExcludeFromCodeCoverage]
    public override string? ToString() => $"{Value} ({Children.Count})";

    public TreeNode(T value)
        => Value = value;
}