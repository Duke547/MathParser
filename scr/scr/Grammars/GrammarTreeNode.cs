using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using MathParsing.Lexing;
using MathParsing.Trees;

namespace MathParsing.Grammars;

public sealed class GrammarTreeNode
{
    readonly TreeNode<GrammarTreeNode> _treeNode;

    public GrammarSymbol Symbol { get; private set; }

    public Token? Token { get; set; }

    internal Queue<ProductionRule> RemainingRules { get; private set; }

    public string Text => Token?.Text ?? Symbol.Description;

    public GrammarTreeNode? Parent
        => _treeNode.Parent?.Value;

    public ImmutableList<GrammarTreeNode> Children
        => _treeNode.Children.Select(c => c.Value).ToImmutableList();

    public void AddChild(GrammarTreeNode child)
        => _treeNode.AddChild(child._treeNode);

    public void Remove() => _treeNode.Remove();

    [ExcludeFromCodeCoverage]
    public override string? ToString() => $"{Symbol} ({Children.Count})";

    public GrammarTreeNode(GrammarSymbol symbol, IEnumerable<ProductionRule> rules)
    {
        _treeNode = new(this);

        Symbol         = symbol;
        RemainingRules = new(rules);
    }
}