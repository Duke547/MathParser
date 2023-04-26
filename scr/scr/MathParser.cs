using MathParsing.Grammars;
using MathParsing.Lexing;

namespace MathParsing;

public static class Parser
{
    private static TokenPattern[] DefineTokenPatterns()
    {
        return new TokenPattern[]
        {
            new("number",            "",                @"\d*\.?\d+"),
            new("add",               "binary operator", @"\+"       ),
            new("subtract",          "binary operator", @"\-"       ),
            new("multiply",          "binary operator", @"[\*\×\∙]" ),
            new("divide",            "binary operator", @"[\/\÷]"   ),
            new("remainder",         "binary operator", @"[\%]"     ),
            new("left parenthesis",  "left bracket",    @"[\(]"     ),
            new("right parenthesis", "right bracket",   @"[\)]"     ),
        };
    }

    private static Grammar DefineGrammar()
    {
        var rules = new Rule[]
        {
            new("number",          true,  true,  new[] { "binary operator", "right bracket"       }),
            new("binary operator", false, false, new[] { "number", "left bracket"                 }),
            new("left bracket",    true,  false, new[] { "number", "left bracket", "right bracket"}),
            new("right bracket",   false, true,  new[] { "binary operator", "right bracket"       }),
        };

        return new(rules);
    }

    private static int GetClosingBracketIndex(List<IMathToken> tokens, int openingBracketIndex)
    {
        var leftBrackets = 0;

        for (int index = openingBracketIndex + 1; index < tokens.Count; index++)
        {
            var token = tokens[index];

            if (token is BracketToken bracketToken)
            {
                if (bracketToken.Left)
                {
                    leftBrackets++;
                }
                else
                {
                    if (leftBrackets == 0)
                    {
                        return index;
                    }
                    else
                    {
                        leftBrackets--;
                    }
                }
            }
        }

        throw new InvalidOperationException("Missing ')'.");
    }

    private static void CollapseBrackets(List<IMathToken> tokens, int index)
    {
        var rightBracketIndex = GetClosingBracketIndex(tokens, index);
        var enclosedTokens    = tokens.GetRange(index + 1, rightBracketIndex - (index + 1));
        var result            = Collapse(enclosedTokens);

        tokens.RemoveRange(index, rightBracketIndex - index + 1);
        tokens.Insert(index, new NumberToken(result));
    }

    private static void CollapseBrackets(List<IMathToken> tokens)
    {
        while (tokens.OfType<BracketToken>().Any())
        {
            var bracketIndex = tokens.FindIndex(token => token is BracketToken bracketToken && bracketToken.Left);

            if (bracketIndex == -1)
                throw new InvalidOperationException("Missing '('.");

            CollapseBrackets(tokens, bracketIndex);
        }
    }

    private static void CollapseBinaryOperator(List<IMathToken> tokens, int index)
    {
        var leftIndex       = index - 1;
        var rightIndex      = index + 1;
        var oper            = (tokens[index     ] as BinaryOperatorToken)!;
        var leftNumber      = (tokens[leftIndex ] as NumberToken        )!;
        var rightNumber     = (tokens[rightIndex] as NumberToken        )!;
        var result          = oper.BinaryOperation.Operation(leftNumber.Value, rightNumber.Value);

        tokens.RemoveAt(rightIndex);
        tokens.RemoveAt(index     );
        tokens.RemoveAt(leftIndex );
        
        tokens.Insert(leftIndex, new NumberToken(result));
    }

    private static void CollapseBinaryOperators(List<IMathToken> tokens)
    {
        while (tokens.OfType<BinaryOperatorToken>().Any())
        {
            var highestPrecedence = tokens
                .OfType<BinaryOperatorToken>()
                .Max(token => token.Precedence);

            var binaryOperatorIndex = tokens.FindIndex(token => token is BinaryOperatorToken binaryOperatorToken &&
                                                                binaryOperatorToken.Precedence == highestPrecedence);

            CollapseBinaryOperator(tokens, binaryOperatorIndex);
        }
    }

    private static decimal Collapse(List<IMathToken> tokens)
    {
        CollapseBrackets(tokens);
        CollapseBinaryOperators(tokens);

        if (tokens.Count == 0)
            return 0;
        else
            return (tokens[0] as NumberToken)!.Value;
    }

    public static decimal Parse(string expression)
    {
        var tokens  = Lexer.Tokenize(expression, DefineTokenPatterns()).ToList();
        var grammar = DefineGrammar();

        grammar.Validate(tokens);

        var mathTokens = MathTokenConverter.Convert(tokens)
                                           .ToList();

        return Collapse(mathTokens);
    }
}