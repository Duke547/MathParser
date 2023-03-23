using MathParsing.Lexing;

namespace MathParsing.Grammars;

public sealed class GrammarTreeBuilder
{
    public Grammar Grammar { get; }

    Token[]          _tokens                    = Array.Empty<Token>();
    Stack<Token>     _remainingTokens           = new();
    GrammarTreeNode? _lastNonTerminalNode       = null;
    int              _furthestTokenIndexReached = -1;
    int              _subsequentNonTerminals    = 0;
    
    public int MaxSubsequentNonTerminals { get; set; } = 3;

    void EvaluateFurthestTokenReached()
    {
        int furthestIndex = _tokens.Length - _remainingTokens.Count - 1;

        if (furthestIndex > _furthestTokenIndexReached)
            _furthestTokenIndexReached = furthestIndex;
    }

    GrammarTreeNode? Build(TerminalSymbol terminal)
    {
        if (_remainingTokens.Peek().Description == terminal.Description)
        {
            var token = _remainingTokens.Pop();
            var tree  = new GrammarTreeNode(terminal, Grammar.Rules) { Token = token };

            EvaluateFurthestTokenReached();
            
            _subsequentNonTerminals = 0;

            return tree;
        }

        return null;
    }

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

    bool ApplyRule(GrammarTreeNode tree, ProductionRule rule)
    {
        if (rule.Replacement.Count > _remainingTokens.Count)
            return false;

        foreach (var replacement in rule.Replacement)
        {
            if (replacement is NonterminalSymbol && _subsequentNonTerminals > MaxSubsequentNonTerminals)
            {
                DiscardChildren(tree);
                return false;
            }

            var child = Build(replacement);

            if (child is null)
            {
                DiscardChildren(tree);
                return false;
            }

            tree.AddChild(child);
        }

        return true;
    }

    bool Build(GrammarTreeNode tree)
    {
        var previousLastNonTerminalNode = _lastNonTerminalNode;

        _subsequentNonTerminals += 1;
        _lastNonTerminalNode     = tree;

        while (true)
        {
            var rule = GetNextProductionRule(tree);

            if (rule is null)
            {
                _subsequentNonTerminals--;
                _lastNonTerminalNode = previousLastNonTerminalNode;
                return false;
            }

            if (!ApplyRule(tree, rule))
                continue;

            return true;
        }
    }

    GrammarTreeNode? Build(GrammarSymbol grammarSymbol)
    {
        if (grammarSymbol is TerminalSymbol terminal)
        {
            return Build(terminal);
        }
        else
        {
            var nonterminal = (grammarSymbol as NonterminalSymbol)!;
            var tree        = new GrammarTreeNode(nonterminal, Grammar.Rules);

            if (Build(tree))
                return tree;
            else
                return null;
        }
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

    void HandleFailure()
    {
        if (_furthestTokenIndexReached > -1)
        {
            var unexpectedTokenIndex = Math.Clamp(_furthestTokenIndexReached + 1, 0, _tokens.Length - 1);
            var unexpectedToken      = _tokens[unexpectedTokenIndex];

            throw new ArgumentException($"Unexpected token '{unexpectedToken}'.");
        }
    }

    public GrammarTreeNode Build(IEnumerable<Token> tokens)
    {
        if (Grammar.Rules.Count == 0)
            throw new InvalidOperationException("The associated grammar does not define any production rules.");

        _tokens          = tokens.ToArray();
        _remainingTokens = new(tokens.Reverse());

        var tree = Build(Grammar.Start);

        if (tree is null)
            HandleFailure();

        while (_remainingTokens.Count > 0)
        {
            DiscardChildren(_lastNonTerminalNode!);

            var result = Build(_lastNonTerminalNode!);
            
            if (!result)
            {
                if (_lastNonTerminalNode!.Parent is null)
                    HandleFailure();

                _lastNonTerminalNode = _lastNonTerminalNode.Parent;
            }
        }

        return tree!;
    }

    public GrammarTreeBuilder(Grammar grammar)
        => Grammar = grammar;
}