using MathParsing.MathTrees;

namespace MathParsing;

public static class Parser
{
    public static decimal Parse(string expression)
        => MathTreeBuilder.Build(expression).Value;
}