using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using MathParsing.Trees;

namespace MathParsing.Grammars;

internal abstract class GrammarTreeNode
{
    protected TreeNode<GrammarTreeNode> TreeNode { get; set; }

    public abstract string Description { get; }

    public abstract string Text { get; }

    public GrammarTreeNode? Parent
        => TreeNode.Parent?.Value;

    public ImmutableList<GrammarTreeNode> Children
        => TreeNode.Children.Select(c => c.Value).ToImmutableList();

    public void AddChild(GrammarTreeNode child)
        => TreeNode.AddChild(child.TreeNode);

    public void Remove() => TreeNode.Remove();

    [ExcludeFromCodeCoverage]
    public override string? ToString() => $"{Description} ({Children.Count})";

    protected GrammarTreeNode()
        => TreeNode = new(this);
}