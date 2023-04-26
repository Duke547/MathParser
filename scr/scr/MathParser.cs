using MathParsing.Grammars;
using MathParsing.Lexing;

namespace MathParsing;

public static class Parser
{
    private static TokenPattern[] DefineTokenPatterns()
    {
        return new TokenPattern[]
        {
            new("number",          @"\d*\.?\d+"),
            new("binary operator", @"[+*]"     ),
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

    private static Func<decimal, decimal, decimal> ConvertToBinaryOperation(Token token) => token.Text switch
    {
        "+" => (l, r) => l + r,
        _   => (l, r) => l * r,
    };

    private static void CollapseBinaryOperator(List<Token> tokens, int index)
    {
        var leftIndex       = index - 1;
        var rightIndex      = index + 1;
        var oper            = tokens[index];
        var leftValue       = int.Parse(tokens[leftIndex] .Text);
        var rightValue      = int.Parse(tokens[rightIndex].Text);
        var binaryOperation = ConvertToBinaryOperation(oper);
        var result          = binaryOperation(leftValue, rightValue);

        tokens.RemoveAt(rightIndex);
        tokens.RemoveAt(index);
        tokens.RemoveAt(leftIndex);
        tokens.Insert(leftIndex, new("number", result.ToString()));
    }

    private static void CollapseBinaryOperators(List<Token> tokens)
    {
        while (tokens.Exists(t => t.Description == "binary operator"))
        {
            var binaryOperatorIndex = tokens.FindIndex(t => t.Description == "binary operator");

            CollapseBinaryOperator(tokens, binaryOperatorIndex);
        }
    }

    public static decimal Parse(string expression)
    {
        var tokens = Lexer.Tokenize(expression, DefineTokenPatterns()).ToList();

        DefineGrammar().Validate(tokens);

        CollapseBinaryOperators(tokens);

        return int.Parse(tokens[0].Text);
    }
}