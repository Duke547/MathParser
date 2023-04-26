using MathParsing.Grammars;
using MathParsing.Lexing;

namespace MathParsing;

public static class Parser
{
    private static TokenPattern[] DefineTokenPatterns()
    {
        return new TokenPattern[]
        {
            new("number",          "",          @"\d*\.?\d+"),
            new("binary operator", "add",       @"\+"       ),
            new("binary operator", "subtract",  @"\-"       ),
            new("binary operator", "multiply",  @"[\*\×\∙]" ),
            new("binary operator", "divide",    @"[\/\÷]"   ),
            new("binary operator", "remainder", @"[\%]"     ),
        };
    }

    private static Grammar DefineGrammar()
    {
        var rules = new Rule[]
        {
            new("number",          true,  true,  new[] { "binary operator" }),
            new("binary operator", false, false, new[] { "number"          }),
        };

        return new(rules);
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
        CollapseBinaryOperators(tokens);

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