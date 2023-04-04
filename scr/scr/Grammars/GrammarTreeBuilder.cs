using System.Collections.Immutable;
using MathParsing.Lexing;

namespace MathParsing.Grammars;

internal sealed class GrammarTreeBuilder
{
    public Grammar Grammar { get; }

    public ImmutableList<TokenPattern> TokenPatterns { get; }

    readonly Stack<NonterminalNode> _processedNonterminalNodes = new();

    Token[]      _tokens                    = Array.Empty<Token>();
    Stack<Token> _remainingTokens           = new();
    int          _furthestTokenIndexReached = -1;
    int          _subsequentNonTerminals    = 0;
    
    public int MaxSubsequentNonTerminals { get; set; } = 3;

    void EvaluateFurthestTokenReached()
    {
        int furthestIndex = _tokens.Length - _remainingTokens.Count - 1;

        if (furthestIndex > _furthestTokenIndexReached)
            _furthestTokenIndexReached = furthestIndex;
    }

    private void PushNonterminalNode(NonterminalNode nonterminalNode)
    {
        _processedNonterminalNodes.Push(nonterminalNode);
        _subsequentNonTerminals++;
    }

    private NonterminalNode PopNonterminalNode()
    {
        if (_subsequentNonTerminals > 0)
            _subsequentNonTerminals--;

        return _processedNonterminalNodes.Pop();
    }

    TerminalNode? Build(TerminalSymbol terminal)
    {
        if (_remainingTokens.Peek().Description == terminal.Description)
        {
            var token = _remainingTokens.Pop();
            var tree  = new TerminalNode(token);

            EvaluateFurthestTokenReached();
            
            _subsequentNonTerminals = 0;

            return tree;
        }

        return null;
    }

    static ProductionRule? GetNextProductionRule(NonterminalNode tree)
    {
        ProductionRule? rule = null;

        while (tree.Rules.Count > 0)
        {
            rule = tree.Rules.Dequeue();

            if (rule.Nonterminal.Description == tree.Description)
                break;
            else
                rule = null;
        }

        return rule;
    }

    bool ApplyRule(NonterminalNode tree, ProductionRule rule)
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

    bool Build(NonterminalNode tree)
    {
        PushNonterminalNode(tree);

        while (true)
        {
            var rule = GetNextProductionRule(tree);

            if (rule is null)
            {
                PopNonterminalNode();
                return false;
            }

            if (!ApplyRule(tree, rule))
                continue;

            return true;
        }
    }

    NonterminalNode? Build(NonterminalSymbol nonterminal)
    {
        var tree = new NonterminalNode(nonterminal.Description, Grammar.Rules);

        if (Build(tree))
            return tree;
        else
            return null;
    }

    GrammarTreeNode? Build(GrammarSymbol grammarSymbol)
    {
        if (grammarSymbol is TerminalSymbol terminal)
            return Build(terminal);
        else
            return Build((grammarSymbol as NonterminalSymbol)!);
    }

    void Discard(GrammarTreeNode tree)
    {
        if (tree is TerminalNode terminalNode)
            _remainingTokens.Push(terminalNode.Token);

        tree.Remove();
    }

    void DiscardChildren(GrammarTreeNode tree)
    {
        foreach (var child in tree.Children.Reverse())
        {
            DiscardChildren(child);
            Discard(child);
        }
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

    public GrammarTreeNode Build(string expression)
    {
        if (Grammar.Rules.Count == 0)
            throw new InvalidOperationException("The associated grammar does not define any production rules.");

        _tokens          = Lexer.Tokenize(expression, TokenPatterns);
        _remainingTokens = new(_tokens.Reverse());

        var tree = Build(Grammar.Start);

        if (tree is null)
            HandleFailure();

        while (_remainingTokens.Count > 0)
        {
            var lastNonterminalNode = PopNonterminalNode();

            DiscardChildren(lastNonterminalNode);

            var result = Build(lastNonterminalNode);
            
            if (!result)
            {
                if (_processedNonterminalNodes.Count == 0)
                    HandleFailure();
            }
        }

        return tree!;
    }

    public GrammarTreeBuilder(Grammar grammar, IEnumerable<TokenPattern> tokenPatterns)
    {
        Grammar       = grammar;
        TokenPatterns = tokenPatterns.ToImmutableList();
    }
}