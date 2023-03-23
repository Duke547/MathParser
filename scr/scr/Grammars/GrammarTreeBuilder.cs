using MathParsing.Lexing;

namespace MathParsing.Grammars;

public sealed class GrammarTreeBuilder
{
    public Grammar Grammar { get; }

    Token[]               _tokens                    = Array.Empty<Token>();
    Stack<Token>          _remainingTokens           = new();
    GrammarTreeNode?      _lastTerminalNode          = null;
    int                   _furthestTokenIndexReached = -1;
    int                   _subsequentNonTerminals    = 0;
    
    readonly int _maxSubsequentNonTerminals = 3;

    static ProductionRule? GetNextProductionRule(GrammarTreeNode tree)
    {
        ProductionRule? rule = null;

        while (tree.RemainingRules.Count > 0)
        {
            rule = tree.RemainingRules.Dequeue();

            if (rule.Nonterminal == tree.Symbol)
                break;
            else
                rule = null;
        }

        return rule;
    }

    bool BuildFromTerminal(GrammarTreeNode tree)
    {
        if (_remainingTokens.First().Description == tree.Symbol.Description)
        {
            var token      = _remainingTokens.Pop();
            var tokenIndex = Array.IndexOf(_tokens, token);

            tree.Token = token;

            _lastTerminalNode = tree;
            
            if (tokenIndex > _furthestTokenIndexReached)
                _furthestTokenIndexReached = tokenIndex;

            _subsequentNonTerminals = 0;

            return true;
        }

        return false;
    }

    bool BuildFromNonterminal(GrammarTreeNode tree)
    {
        var rule = GetNextProductionRule(tree);

        if (rule is null || rule.Replacement.Count > _remainingTokens.Count || _subsequentNonTerminals > _maxSubsequentNonTerminals)
            return false;

        _subsequentNonTerminals++;

        foreach (var replacement in rule.Replacement)
        {
            var child = new GrammarTreeNode(replacement, Grammar.Rules);

            if (!Build(child))
            {
                DiscardChildren(tree);

                _subsequentNonTerminals--;

                return BuildFromNonterminal(tree);
            }

            tree.AddChild(child);
        }

        return true;
    }

    void Discard(GrammarTreeNode tree)
    {
        if (tree.Symbol is TerminalSymbol)
            _remainingTokens.Push(tree.Token!);

        tree.Remove();
    }

    void DiscardChildren(GrammarTreeNode tree)
    {
        foreach (var child in tree.Children.Reverse())
            Discard(child);
    }

    bool Build(GrammarTreeNode tree)
    {
        if (tree.Symbol is TerminalSymbol)
            return BuildFromTerminal(tree);
        else
            return BuildFromNonterminal(tree);
    }

    void HandleFailure()
    {
        if (_furthestTokenIndexReached > -1)
        {
            var unexpectedToken = _tokens[_furthestTokenIndexReached + 1];

            throw new ArgumentException($"Unexpected token '{unexpectedToken}'.");
        }
    }

    public GrammarTreeNode Build(IEnumerable<Token> tokens)
    {
        _tokens          = tokens.ToArray();
        _remainingTokens = new(tokens.Reverse());

        var tree   = new GrammarTreeNode(Grammar.Start, Grammar.Rules);
        var result = Build(tree);

        while (result && _remainingTokens.Count > 0)
        {
            var lastTerminalParent = _lastTerminalNode!.Parent!;

            Discard(_lastTerminalNode);

            result = Build(lastTerminalParent);
        }

        if (!result)
            HandleFailure();

        return tree;
    }

    public GrammarTreeBuilder(Grammar grammar)
        => Grammar = grammar;
}