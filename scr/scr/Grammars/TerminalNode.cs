using MathParsing.Lexing;
using MathParsing.Trees;

namespace MathParsing.Grammars;

internal class TerminalNode : GrammarTreeNode
{
    public Token Token { get; init; }

    public override string Description => Token.Description;

    public override string Text => Token.Text;

    public TerminalNode(Token token)
    {
        TreeNode = new LeafTreeNode<GrammarTreeNode>(this);
        Token    = token;
    }
}