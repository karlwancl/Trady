using System;
using System.Linq;
using Trady.Core;

namespace Trady.Analysis.Indicator
{
    public partial class SimpleMovingAverage : IndicatorBase
    {
        private const string SmaTag = "Sma";

        public SimpleMovingAverage(Equity series, int periodCount) : base(series, periodCount)
        {
        }

        public int PeriodCount => Parameters[0];

        protected override IAnalyticResult<decimal> ComputeResultByIndex(int index)
        {
            decimal sma = index == 0 ? 0 : Series.Skip(index - PeriodCount + 1).Take(PeriodCount).Sum(c => c.Close) / PeriodCount;
            return new IndicatorResult(Series[index].DateTime, sma);
        }

        public IndicatorResultTimeSeries<IndicatorResult> Compute(DateTime? startTime = null, DateTime? endTime = null)
            => new IndicatorResultTimeSeries<IndicatorResult>(Series.Name, ComputeResults<IndicatorResult>(startTime, endTime), Series.Period, Series.MaxTickCount);

        public IndicatorResult ComputeByDateTime(DateTime dateTime)
            => ComputeResultByDateTime<IndicatorResult>(dateTime);

        public IndicatorResult ComputeByIndex(int index)
            => ComputeResultByIndex<IndicatorResult>(index);
    }
}
