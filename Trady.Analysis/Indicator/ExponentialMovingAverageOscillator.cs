using System;
using Trady.Core;

namespace Trady.Analysis.Indicator
{
    public partial class ExponentialMovingAverageOscillator : IndicatorBase
    {
        private const string OscTag = "Osc";

        private ExponentialMovingAverage _emaIndicator1, _emaIndicator2;

        public ExponentialMovingAverageOscillator(Equity series, int periodCount1, int periodCount2) 
            : base(series, periodCount1, periodCount2)
        {
            _emaIndicator1 = new ExponentialMovingAverage(series, periodCount1);
            _emaIndicator2 = new ExponentialMovingAverage(series, periodCount2);
        }

        public int PeriodCount1 => Parameters[0];

        public int PeriodCount2 => Parameters[1];

        protected override IAnalyticResult<decimal> ComputeResultByIndex(int index)
        {
            var osc = _emaIndicator1.ComputeByIndex(index).Ema - _emaIndicator2.ComputeByIndex(index).Ema;
            return new IndicatorResult(Series[index].DateTime, osc);
        }

        public IndicatorResultTimeSeries<IndicatorResult> Compute(DateTime? startTime = null, DateTime? endTime = null)
            => new IndicatorResultTimeSeries<IndicatorResult>(Series.Name, ComputeResults<IndicatorResult>(startTime, endTime), Series.Period, Series.MaxTickCount);

        public IndicatorResult ComputeByDateTime(DateTime dateTime)
            => ComputeResultByDateTime<IndicatorResult>(dateTime);

        public IndicatorResult ComputeByIndex(int index)
            => ComputeResultByIndex<IndicatorResult>(index);
    }
}
