using System;
using Trady.Core;

namespace Trady.Analysis.Indicator
{
    public partial class ClosePriceChange : IndicatorBase
    {
        private const string ChangeTag = "Change";

        public ClosePriceChange(Equity series) : base(series, null)
        {
        }

        protected override IAnalyticResult<decimal> ComputeResultByIndex(int index)
           => new IndicatorResult(Series[index].DateTime, index == 0 ? 0 : Series[index].Close - Series[index - 1].Close);

        public IndicatorResultTimeSeries<IndicatorResult> Compute(DateTime? startTime = null, DateTime? endTime = null)
            => new IndicatorResultTimeSeries<IndicatorResult>(Series.Name, ComputeResults<IndicatorResult>(startTime, endTime), Series.Period, Series.MaxTickCount);

        public IndicatorResult ComputeByDateTime(DateTime dateTime)
            => ComputeResultByDateTime<IndicatorResult>(dateTime);

        public IndicatorResult ComputeByIndex(int index)
            => ComputeResultByIndex<IndicatorResult>(index);
    }
}
