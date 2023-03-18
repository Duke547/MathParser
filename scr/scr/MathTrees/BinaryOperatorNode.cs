namespace MathParsing.MathTrees;

public sealed class BinaryOperatorNode : MathTreeNode
{
    public string Symbol { get; private set; }

    public MathTreeNode? LeftChild => Children[0];
    
    public MathTreeNode? RightChild => Children[1];

    public Func<decimal, decimal, decimal> Operation { get; private set; }

    public override decimal Value
    {
        get
        {
            if (LeftChild == null || RightChild == null)
                return 0;

            return Operation(LeftChild.Value, RightChild.Value);
        }
    }

    public override bool Open => Children.Count < 2;

    public override void AddChild(MathTreeNode child)
    {
        if (!Open)
            throw new InvalidOperationException("This binary operator node already has two children.");

        base.AddChild(child);
    }

    public BinaryOperatorNode(string symbol, Func<decimal, decimal, decimal> operation)
    {
        Symbol    = symbol;
        Operation = operation;
    }
}