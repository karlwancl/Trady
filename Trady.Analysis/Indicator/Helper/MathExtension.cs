using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Trady.Analysis.Indicator.Helper
{
    public static class MathExtension
    {
        public static decimal Sd(this IEnumerable<decimal> values)
        {
            var mean = values.Average();
            var sd = Math.Sqrt(values.Select(v => Math.Pow((double)(v - mean), 2)).Sum() / (values.Count() - 1));
            return Convert.ToDecimal(sd);
        }
    }
}
