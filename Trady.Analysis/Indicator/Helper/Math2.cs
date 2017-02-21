using System;
using System.Collections.Generic;
using System.Linq;

namespace Trady.Analysis.Indicator.Helper
{
    internal static class Math2
    {
        public static decimal Sd(this IEnumerable<decimal> values)
        {
            var mean = values.Average();
            var sd = Math.Sqrt(values.Select(v => Math.Pow((double)(v - mean), 2)).Sum() / (values.Count() - 1));
            return Convert.ToDecimal(sd);
        }

        public static decimal? Abs(this decimal? value)
            => value.HasValue ? Math.Abs(value.Value) : (decimal?)null;

        public static decimal? Max(decimal? value1, decimal? value2)
            => (value1.HasValue && value2.HasValue) ? Math.Max(value1.Value, value2.Value) : (decimal?)null;

        public static decimal? Min(decimal? value1, decimal? value2)
            => (value1.HasValue && value2.HasValue) ? Math.Min(value1.Value, value2.Value) : (decimal?)null;
    }
}