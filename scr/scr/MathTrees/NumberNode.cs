namespace MathParsing.MathTrees;

public sealed class NumberNode : MathTreeNode
{
    readonly decimal _value;

    public override decimal Value => _value;

    public override bool Open => false;

    public NumberNode(decimal value)
        => _value = value;
}