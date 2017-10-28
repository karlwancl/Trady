using System;
using System.Collections.Generic;
using System.Linq;

namespace Trady.Analysis.Extension
{
    internal static class TypeExtension
    {
        public static int? FindIndexOrDefault<T>(this IEnumerable<T> list, Predicate<T> predicate, int? defaultValue = null)
        {
            int index = list.ToList().FindIndex(predicate);
            return index == -1 ? defaultValue : index;
        }

        public static int? FindLastIndexOrDefault<T>(this IEnumerable<T> list, Predicate<T> predicate, int? defaultValue = null)
        {
            // TODO: May have performance issue here
            int index = list.ToList().FindLastIndex(predicate);
            return index == -1 ? defaultValue : index;
        }
    }
}