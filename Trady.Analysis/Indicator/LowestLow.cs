using System;
using System.Linq;
using Trady.Core;

namespace Trady.Analysis.Indicator
{
    public partial class LowestLow : IndicatorBase
    {
        private const string LowestLowTag = "LowestLow";

        public LowestLow(Equity series, int periodCount) : base(series, periodCount)
        {
        }

        public int PeriodCount => Parameters[0];

        protected override IAnalyticResult<decimal> ComputeResultByIndex(int index)
        {
            var lowestLow = Series.Skip(index - PeriodCount + 1).Take(PeriodCount).Min(c => c.Low);
            return new IndicatorResult(Series[index].DateTime, lowestLow);
        }

        public IndicatorResultTimeSeries<IndicatorResult> Compute(DateTime? startTime = null, DateTime? endTime = null)
            => new IndicatorResultTimeSeries<IndicatorResult>(Series.Name, ComputeResults<IndicatorResult>(startTime, endTime), Series.Period, Series.MaxTickCount);

        public IndicatorResult ComputeByDateTime(DateTime dateTime)
            => ComputeResultByDateTime<IndicatorResult>(dateTime);

        public IndicatorResult ComputeByIndex(int index)
            => ComputeResultByIndex<IndicatorResult>(index);
    }
}
