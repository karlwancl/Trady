using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Trady.Core;

namespace Trady.Strategy.Helper
{
    public static class EquityExtension
    {
        internal static ComputableCandle GetComputableCandleAt(this Equity equity, int index)
            => new ComputableCandle(equity, index);

        public static IList<ComputableCandle> ToComputableCandles(this Equity equity)
            => Enumerable.Range(0, equity.TickCount).Select(i => new ComputableCandle(equity, i)).ToList();
    }
}
