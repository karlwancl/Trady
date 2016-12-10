using System;
using System.Collections.Generic;
using System.Text;
using Trady.Core;

namespace Trady.Strategy.Helper
{
    internal static class EquityExtension
    {
        public static EquityCandle GetCandleAt(this Equity equity, int index)
            => new EquityCandle(equity, index);
    }
}
