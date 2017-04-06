using System.Collections.Generic;
using System.Linq;
using Trady.Core;

namespace Trady.Analysis.Indicator
{
    public partial class SimpleMovingAverage : IndicatorBase<decimal, decimal?>
    {
        public SimpleMovingAverage(IList<Candle> candles, int periodCount)
            : this(candles.Select(c => c.Close).ToList(), periodCount)
        {
        }

        public SimpleMovingAverage(IList<decimal> closes, int periodCount) : base(closes, periodCount)
        {
        }

        public int PeriodCount => Parameters[0];

        protected override decimal? ComputeByIndexImpl(int index)
            => index >= PeriodCount - 1 ? Inputs.Skip(index - PeriodCount + 1).Take(PeriodCount).Average() : (decimal?)null;
    }
}