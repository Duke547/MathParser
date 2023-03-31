using System.Runtime.CompilerServices;

[assembly:InternalsVisibleTo("MathParsing.Testing")]

namespace MathParsing.Grammars;

public class NonterminalNode : GrammarTreeNode
{
    public override string Description { get; }

    public override string Text => "";

    internal Queue<ProductionRule> Rules { get; init; }

    public NonterminalNode(string description, IEnumerable<ProductionRule> rules)
    {
        Description = description;
        Rules       = new(rules);
    }
}