using System;
using System.Linq;
using Trady.Core;

namespace Trady.Analysis.Indicator
{
    public partial class HighestHigh : IndicatorBase
    {
        private const string HighestHighTag = "HighestHigh";

        public HighestHigh(Equity series, int periodCount) : base(series, periodCount)
        {
        }

        public int PeriodCount => Parameters[0];

        protected override IAnalyticResult<decimal> ComputeResultByIndex(int index)
        {
            var highestHigh = Series.Skip(index - PeriodCount + 1).Take(PeriodCount).Max(c => c.High);
            return new IndicatorResult(Series[index].DateTime, highestHigh);
        }

        public IndicatorResultTimeSeries<IndicatorResult> Compute(DateTime? startTime = null, DateTime? endTime = null)
            => new IndicatorResultTimeSeries<IndicatorResult>(Series.Name, ComputeResults<IndicatorResult>(startTime, endTime), Series.Period, Series.MaxTickCount);

        public IndicatorResult ComputeByDateTime(DateTime dateTime)
            => ComputeResultByDateTime<IndicatorResult>(dateTime);

        public IndicatorResult ComputeByIndex(int index)
            => ComputeResultByIndex<IndicatorResult>(index);
    }
}
