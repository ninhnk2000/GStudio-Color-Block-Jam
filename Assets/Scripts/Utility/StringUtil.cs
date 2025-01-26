using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public static class StringUtil
{
    public static string AddSpaceBeforeUppercase(string input)
    {
        StringBuilder result = new StringBuilder();

        foreach (char c in input)
        {
            if (char.IsUpper(c) && result.Length > 0)
            {
                result.Append(" ");
            }

            result.Append(c);
        }

        return result.ToString();
    }
}
