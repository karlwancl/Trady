using System.Collections.Generic;
using System.Linq;
using Trady.Core;

namespace Trady.Analysis.Indicator
{
    public partial class LowestLow : IndicatorBase<decimal, decimal?>
    {
        public LowestLow(IList<Candle> candles, int periodCount) :
            this(candles.Select(c => c.Low).ToList(), periodCount)
        {
        }

        public LowestLow(IList<decimal> lows, int periodCount) : base(lows, periodCount)
        {
        }

        public int PeriodCount => Parameters[0];

        protected override decimal? ComputeByIndexImpl(int index)
            => index >= PeriodCount - 1 ? Inputs.Skip(index - PeriodCount + 1).Take(PeriodCount).Min() : (decimal?)null;
    }
}