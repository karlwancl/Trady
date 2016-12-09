using System;
using Trady.Core;

namespace Trady.Analysis.Indicator
{
    public partial class RelativeStrengthIndex : IndicatorBase
    {
        private const string RsiTag = "Rsi";

        private RelativeStrength _rsIndicator;

        public RelativeStrengthIndex(Equity series, int periodCount) : base(series, periodCount)
        {
            _rsIndicator = new RelativeStrength(series, periodCount);
        }

        public int PeriodCount => Parameters[0];

        protected override IAnalyticResult<decimal> ComputeResultByIndex(int index)
        {
            var rsi = 100 - (100 / (1 + _rsIndicator.ComputeByIndex(index).Rs));
            return new IndicatorResult(Series[index].DateTime, rsi);
        }

        public IndicatorResultTimeSeries<IndicatorResult> Compute(DateTime? startTime = null, DateTime? endTime = null)
            => new IndicatorResultTimeSeries<IndicatorResult>(Series.Name, ComputeResults<IndicatorResult>(startTime, endTime), Series.Period, Series.MaxTickCount);

        public IndicatorResult ComputeByDateTime(DateTime dateTime)
            => ComputeResultByDateTime<IndicatorResult>(dateTime);

        public IndicatorResult ComputeByIndex(int index)
            => ComputeResultByIndex<IndicatorResult>(index);
    }
}
