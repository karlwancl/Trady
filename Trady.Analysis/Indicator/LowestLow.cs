using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Infrastructure;
using Trady.Core;

namespace Trady.Analysis.Indicator
{
    public partial class LowestLow : AnalyzableBase<decimal, decimal?>
    {
        public LowestLow(IList<Candle> candles, int periodCount) :
            this(candles.Select(c => c.Low).ToList(), periodCount)
        {
        }

        public LowestLow(IList<decimal> lows, int periodCount) : base(lows)
        {
            PeriodCount = periodCount;
        }

        public int PeriodCount { get; private set; }

        protected override decimal? ComputeByIndexImpl(int index)
            => index >= PeriodCount - 1 ? Inputs.Skip(index - PeriodCount + 1).Take(PeriodCount).Min() : (decimal?)null;
    }
}