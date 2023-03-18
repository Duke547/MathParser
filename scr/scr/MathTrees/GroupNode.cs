namespace MathParsing.MathTrees;

public sealed class GroupNode : MathTreeNode
{
    public override decimal Value
        => Children.FirstOrDefault() != null ? Children.First().Value : 0;

    public override bool Open => Children.Count < 1;
}