using System;
using Trady.Core;

namespace Trady.Analysis.Indicator
{
    public partial class ClosePriceChange : IndicatorBase
    {
        private const string ChangeTag = "Change";

        public ClosePriceChange(Equity equity) : base(equity, null)
        {
        }

        protected override IAnalyticResult<decimal> ComputeResultByIndex(int index)
           => new IndicatorResult(Equity[index].DateTime, index == 0 ? 0 : Equity[index].Close - Equity[index - 1].Close);

        public IndicatorResultTimeSeries<IndicatorResult> Compute(DateTime? startTime = null, DateTime? endTime = null)
            => new IndicatorResultTimeSeries<IndicatorResult>(Equity.Name, ComputeResults<IndicatorResult>(startTime, endTime), Equity.Period, Equity.MaxTickCount);

        public IndicatorResult ComputeByDateTime(DateTime dateTime)
            => ComputeResultByDateTime<IndicatorResult>(dateTime);

        public IndicatorResult ComputeByIndex(int index)
            => ComputeResultByIndex<IndicatorResult>(index);
    }
}
