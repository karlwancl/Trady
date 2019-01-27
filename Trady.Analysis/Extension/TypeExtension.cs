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

        public static TReturn GetOrAdd<TKey,TReturn>(this IDictionary<TKey, TReturn> keyValuePairs, TKey key, Func<TReturn> instanceFunction)
        {
            if (!keyValuePairs.TryGetValue(key, out var @return))
            {
                @return = instanceFunction();
                keyValuePairs.Add(key, @return);
            }
            return @return;
        }

        public static TReturn GetOrAdd<TKey, TReturn>(this IDictionary<TKey, TReturn> keyValuePairs, TKey key, Func<TKey, TReturn> instanceFunction)
        {
            if (!keyValuePairs.TryGetValue(key, out var @return))
            {
                @return = instanceFunction(key);
                keyValuePairs.Add(key, @return);
            }
            return @return;
        }

        public static void AddOrUpdate<TKey, TReturn>(this IDictionary<TKey, TReturn> keyValuePairs, TKey key, TReturn value, Func<TKey, TReturn, TReturn> updateFactory)
        {
            if (!keyValuePairs.TryGetValue(key, out var @return))
            {
                @return = value;
                keyValuePairs.Add(key, @return);
            }
            else
            {
                @return = updateFactory(key, @return);
                keyValuePairs[key] = @return;
            }
        }
    }
}
