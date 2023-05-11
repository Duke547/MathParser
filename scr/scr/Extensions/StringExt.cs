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

    /// <summary>
    /// Removes the first occurance of the specified substring.
    /// </summary>
    /// <param name="str">The current string.</param>
    /// <param name="substring">The substring to remove.</param>
    /// <returns>A new string with the first occurance of the specified substring removed.</returns>
    public static string RemoveFirst(this string str, string substring)
    {
        for (int i = 0; i < str.Length; i++)
        {
            if (i + substring.Length > str.Length)
                break;

            if (str.Substring(i, substring.Length) == substring)
                return str.Remove(i, substring.Length);
        }

        return str;
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