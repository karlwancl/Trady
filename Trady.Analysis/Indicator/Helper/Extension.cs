using System;
using System.Collections.Generic;

namespace Trady.Analysis.Indicator.Helper
{
    public static class Extension
    {
        public static int? FindIndexOrDefault<T>(this List<T> list, Predicate<T> predicate)
        {
            int index = list.FindIndex(predicate);
            return index == -1 ? (int?)null : index;
        }

        public static int? FindLastIndexOrDefault<T>(this List<T> list, Predicate<T> predicate)
        {
            int index = list.FindLastIndex(predicate);
            return index == -1 ? (int?)null : index;
        }
    }
}