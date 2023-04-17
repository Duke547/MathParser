using MathParsing.Grammars;
using MathParsing.Lexing;

namespace MathParsing.MathTrees;

internal static class MathTreeBuilder
{
    private static TokenPattern[] DefineTokenPatterns()
    {
        return new TokenPattern[]
        {
            new("number",   @"\d*\.?\d+"),
            new("add",      @"\+"       ),
            new("multiply", @"\*"       )
        };
    }

    private static Grammar DefineGrammar()
    {
        var rules = new Rule[]
        {
            new("number",   true,  true,  new[] { "add", "multiply" }),
            new("add",      false, false, new[] { "number"          }),
            new("multiply", false, false, new[] { "number"          })
        };

        return new(rules);
    }

    private static NumberNode ConvertToNumberNode(Token token)
    {
        var number = decimal.Parse(token.Text);

        return new(number);
    }

    private static Func<decimal, decimal, decimal> ConvertToBinaryOperation(string description) => description switch
    {
        "add" => (l, r) => l + r,
        _     => (l, r) => l * r,
    };

    private static BinaryOperatorNode ConvertToBinaryOperatorNode(Token token)
    {
        var symbol    = token.Text;
        var operation = ConvertToBinaryOperation(token.Description);

        return new(symbol, operation);
    }

    private static MathTreeNode ConvertToMathNode(Token token)
    {
        if (token.Description == "number")
            return ConvertToNumberNode(token);
        else
            return ConvertToBinaryOperatorNode(token);
    }

    private static void Attach(MathTreeNode last, MathTreeNode next)
    {
        if (last.IsOpen)
        {
            last.AddChild(next);
        }
        else
        {
            var lastParent = last.Parent!;

            last      .Remove();
            lastParent.AddChild(next);
            next      .AddChild(last);
        }
    }

    public static MathTreeNode Build(string expression)
    {
        var tokens = Lexer.Tokenize(expression, DefineTokenPatterns());
        
        DefineGrammar().Validate(tokens);

        MathTreeNode lastNode = new GroupNode();

        foreach (var token in tokens)
        {
            var node = ConvertToMathNode(token);

            Attach(lastNode, node);

            lastNode = node;
        }

        return lastNode.Root;
    }
}