using System.Collections;

namespace MathParsing.Extensions;

internal static class StringExt
{
    public static string RemoveWhitespace(this string str)
    {
        var newString = "";
        
        foreach (var character in str)
        {
            if (!char.IsWhiteSpace(character))
                newString += character;
        }

        return newString;
    }

    public static string FromList(IList list)
    {
        var str = "";

        foreach (var item in list)
        {
            str += item.ToString();

            if (list.IndexOf(item) < list.Count - 1)
                str += " ";
        }

        return str;
    }
}