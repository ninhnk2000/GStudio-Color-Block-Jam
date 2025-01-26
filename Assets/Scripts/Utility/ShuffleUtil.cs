using System.Collections.Generic;
using UnityEngine;

public static class ShuffleUtil
{
    public static Dictionary<T1, T2> ShuffleItemsWithSameValue<T1, T2>(Dictionary<T1, T2> dict)
    {
        var valueGroups = new Dictionary<T2, List<T1>>();

        foreach (var kvp in dict)
        {
            if (!valueGroups.ContainsKey(kvp.Value))
            {
                valueGroups[kvp.Value] = new List<T1>();
            }
            valueGroups[kvp.Value].Add(kvp.Key);
        }

        var rng = new System.Random();
        foreach (var group in valueGroups.Values)
        {
            ShuffleList(group, rng);
        }

        var shuffledDict = new Dictionary<T1, T2>();

        foreach (var kvp in dict)
        {
            var shuffledKey = valueGroups[kvp.Value][0];
            shuffledDict[shuffledKey] = kvp.Value;
            valueGroups[kvp.Value].RemoveAt(0);
        }

        return shuffledDict;
    }

    private static void ShuffleList<T>(List<T> list, System.Random rng)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}
