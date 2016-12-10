using System;
using Trady.Core;

namespace Trady.Analysis.Indicator
{
    public partial class RawStochasticsValue : IndicatorBase
    {
        private const string RsvTag = "Rsv";

        private HighestHigh _highestHighIndicator;
        private LowestLow _lowestLowIndicator;

        public RawStochasticsValue(Equity equity, int periodCount) : base(equity, periodCount)
        {
            _highestHighIndicator = new HighestHigh(equity, periodCount);
            _lowestLowIndicator = new LowestLow(equity, periodCount);
        }

        public int PeriodCount => Parameters[0];

        protected override IAnalyticResult<decimal> ComputeResultByIndex(int index)
        {
            decimal rsv = 50;
            if (index >= PeriodCount - 1)
            {
                var highestHigh = _highestHighIndicator.ComputeByIndex(index).HighestHigh;
                var lowestLow = _lowestLowIndicator.ComputeByIndex(index).LowestLow;
                rsv = highestHigh == lowestLow ? 50 : 100 * (Equity[index].Close - lowestLow) / (highestHigh - lowestLow);
            }
            return new IndicatorResult(Equity[index].DateTime, rsv);
        }

        public IndicatorResultTimeSeries<IndicatorResult> Compute(DateTime? startTime = null, DateTime? endTime = null)
            => new IndicatorResultTimeSeries<IndicatorResult>(Equity.Name, ComputeResults<IndicatorResult>(startTime, endTime), Equity.Period, Equity.MaxTickCount);

        public IndicatorResult ComputeByDateTime(DateTime dateTime)
            => ComputeResultByDateTime<IndicatorResult>(dateTime);

        public IndicatorResult ComputeByIndex(int index)
            => ComputeResultByIndex<IndicatorResult>(index);
    }
}
