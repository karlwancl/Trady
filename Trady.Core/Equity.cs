using System.Collections.Generic;
using Trady.Core.Period;

namespace Trady.Core
{
    public class Equity : TimeSeries<Candle>
    {
        public Equity(string name, IEnumerable<Candle> candles, PeriodOption period, int? maxTickCount)
            : base(name, candles, period, maxTickCount)
        {
        }
    }
}