using MathParsing.Lexing;

namespace MathParsing.Grammars;

public sealed class GrammarTreeBuilder
{
    public Grammar Grammar { get; }

    Stack<Token> RemainingTokens { get; set; } = new();

    GrammarTreeNode? LastTerminalNode { get; set; } = default;

    bool BuildFromTerminal(GrammarTreeNode tree)
    {
        if (RemainingTokens.Count > 0 && RemainingTokens.First().Description == tree.Symbol.Description)
        {
            var token = RemainingTokens.Pop();
            
            tree.Token = token;

            LastTerminalNode = tree;

            return true;
        }

        return false;
    }

    bool BuildFromNonterminal(GrammarTreeNode tree)
    {
        ProductionRule? rule = default;

        while (tree.RemainingRules.Count > 0)
        {
            rule = tree.RemainingRules.Dequeue();

            if (rule.Nonterminal == tree.Symbol)
                break;
            else
                rule = null;
        }

        if (rule is null || rule.Replacement.Count > RemainingTokens.Count)
            return false;

        foreach (var replacement in rule.Replacement)
        {
            var child = new GrammarTreeNode(replacement, Grammar.Rules);

            if (!Build(child))
            {
                DiscardChildren(tree);

                return BuildFromNonterminal(tree);
            }

            tree.AddChild(child);
        }

        return true;
    }

    void Discard(GrammarTreeNode tree)
    {
        if (tree.Symbol.Terminal)
            RemainingTokens.Push(tree.Token!);

        tree.Remove();
    }

    void DiscardChildren(GrammarTreeNode tree)
    {
        foreach (var child in tree.Children.Reverse())
            Discard(child);
    }

    bool Build(GrammarTreeNode tree)
    {
        if (tree.Symbol.Terminal)
            return BuildFromTerminal(tree);
        else
            return BuildFromNonterminal(tree);
    }

    public GrammarTreeNode Build(IEnumerable<Token> tokens)
    {
        RemainingTokens = new(tokens);

        var tree   = new GrammarTreeNode(Grammar.Start, Grammar.Rules);
        var result = Build(tree);

        while (result && RemainingTokens.Count > 0)
        {
            var lastTerminalParent = LastTerminalNode!.Parent!;

            Discard(LastTerminalNode);

            result = Build(lastTerminalParent);
        }

        if (!result)
            throw new InvalidOperationException($"Unexpected token {RemainingTokens.First()}.");

        return tree;
    }

    public GrammarTreeBuilder(Grammar grammar)
        => Grammar = grammar;
}