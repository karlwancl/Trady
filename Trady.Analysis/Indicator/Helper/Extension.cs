using System;
using System.Collections.Generic;

namespace Trady.Analysis.Indicator.Helper
{
    public static class Extension
    {
        internal static int? FindIndexOrDefault<T>(this List<T> list, Predicate<T> predicate)
        {
            int index = list.FindIndex(predicate);
            return index == -1 ? (int?)null : index;
        }

        internal static int? FindLastIndexOrDefault<T>(this List<T> list, Predicate<T> predicate)
        {
            int index = list.FindLastIndex(predicate);
            return index == -1 ? (int?)null : index;
        }

        internal static void AddOrUpdate<T1, T2>(this IDictionary<T1, T2> dict, T1 item1, T2 item2)
        {
            if (dict.ContainsKey(item1))
                dict[item1] = item2;
            else
                dict.Add(item1, item2);
        }
    }
}