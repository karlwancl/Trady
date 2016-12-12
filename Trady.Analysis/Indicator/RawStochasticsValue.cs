using System;
using Trady.Core;
using static Trady.Analysis.Indicator.RawStochasticsValue;

namespace Trady.Analysis.Indicator
{
    public partial class RawStochasticsValue : IndicatorBase<IndicatorResult>
    {
        private HighestHigh _highestHighIndicator;
        private LowestLow _lowestLowIndicator;

        public RawStochasticsValue(Equity equity, int periodCount) : base(equity, periodCount)
        {
            _highestHighIndicator = new HighestHigh(equity, periodCount);
            _lowestLowIndicator = new LowestLow(equity, periodCount);
        }

        public int PeriodCount => Parameters[0];

        public override IndicatorResult ComputeByIndex(int index)
        {
            var highestHigh = _highestHighIndicator.ComputeByIndex(index).HighestHigh;
            var lowestLow = _lowestLowIndicator.ComputeByIndex(index).LowestLow;
            var rsv = (highestHigh == lowestLow) ? 50 : 100 * (Equity[index].Close - lowestLow) / (highestHigh - lowestLow);
            return new IndicatorResult(Equity[index].DateTime, rsv);
        }
    }
}
