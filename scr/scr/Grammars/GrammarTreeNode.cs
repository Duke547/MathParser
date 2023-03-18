using System.Collections.Immutable;
using MathParsing.Lexing;
using MathParsing.Trees;

namespace MathParsing.Grammars;

public sealed class GrammarTreeNode
{
    readonly TreeNode<GrammarTreeNode> _treeNode;

    public GrammarSymbol Symbol { get; private set; }

    internal Token? Token { get; set; }

    internal Queue<ProductionRule> RemainingRules { get; private set; }

    public string Text => Token?.Text ?? "";

    public GrammarTreeNode? Parent
        => _treeNode.Parent?.Value as GrammarTreeNode;

    public ImmutableList<GrammarTreeNode> Children
        => _treeNode.Children.Select(c => (c.Value as GrammarTreeNode)!).ToImmutableList();

    public void AddChild(GrammarTreeNode child)
        => _treeNode.AddChild(child._treeNode);

    public void Remove() => _treeNode.Remove();

    public override string? ToString() => $"{Symbol} ({Children.Count})";

    public GrammarTreeNode(GrammarSymbol symbol, IEnumerable<ProductionRule> rules)
    {
        _treeNode = new(this);

        Symbol         = symbol;
        RemainingRules = new(rules);
    }
}