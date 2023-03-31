using MathParsing.Lexing;

namespace MathParsing;

internal static class TokenPatterns
{
    public static readonly TokenPattern[] All = new TokenPattern[]
    {
        new("number",         @"\d*\.?\d+"),
        new("addition",       @"\+"       ),
        new("multiplication", @"\*"       )
    };
}