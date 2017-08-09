using System;
using System.Collections.Generic;
using System.Linq;

namespace Trady.Analysis.Helper
{
    static class Misc
    {
        static object syncLock = new object();

        public static int? FindIndexOrDefault<T>(this IEnumerable<T> list, Predicate<T> predicate, int? defaultValue = null)
        {
            int index = list.ToList().FindIndex(predicate);
            return index == -1 ? defaultValue : index;
        }

        public static int? FindLastIndexOrDefault<T>(this IEnumerable<T> list, Predicate<T> predicate, int? defaultValue = null)
        {
            int index = list.ToList().FindLastIndex(predicate);
            return index == -1 ? defaultValue : index;
        }

        public static void AddOrUpdate<T1, T2>(this IDictionary<T1, T2> dict, T1 item1, T2 item2)
        {
            lock (syncLock)
            {
                if (dict.ContainsKey(item1))
                    dict[item1] = item2;
                else
                    dict.Add(item1, item2);
            }
        }

        public static decimal? Avg(this IEnumerable<decimal> values, int periodCount, int index)
            => index >= periodCount - 1 ? values.Skip(index - periodCount + 1).Take(periodCount).Average() : (decimal?)null;

        public static decimal? Sd(this IEnumerable<decimal> values, int periodCount, int index)
        {
            if (index < periodCount - 1)
                return null;

            var vs = values.Skip(index - periodCount + 1).Take(periodCount);
            decimal avg = vs.Average();
            decimal diffSum = vs.Select(v => (v - avg) * (v - avg)).Sum();
            return Convert.ToDecimal(Math.Sqrt(Convert.ToDouble(diffSum / (vs.Count() - 1))));
        }

        public static decimal? Median(this IList<decimal> values, int periodCount, int index)
            => Percentile(values, periodCount, index, 0.5m);

        public static decimal? Percentile(this IEnumerable<decimal> values, int periodCount, int index, decimal percentile)
        {
            if (percentile < 0 || percentile > 1)
                throw new ArgumentException("Percentile should be between 0 and 1", nameof(percentile));

            if (index < periodCount - 1)
                return null;

            var subset = values.Skip(index - periodCount + 1).Take(periodCount).OrderBy(v => v).ToList();
            var idx = percentile * (subset.Count - 1) + 1;

            if (idx == 1) return subset[0];
            if (idx == subset.Count) return subset.Last();

            return subset[(int)idx - 1] + (subset[(int)idx] - subset[(int)idx - 1]) * (idx - (int)idx);
        }
    }
}