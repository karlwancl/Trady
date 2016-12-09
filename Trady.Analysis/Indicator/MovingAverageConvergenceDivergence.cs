using System;
using Trady.Core;

namespace Trady.Analysis.Indicator
{
    public partial class MovingAverageConvergenceDivergence : CachedIndicatorBase
    {
        private const string DifTag = "Dif";
        private const string DemTag = "Dem";
        private const string OscTag = "Osc";

        private ExponentialMovingAverage _emaIndicator1, _emaIndicator2;

        public MovingAverageConvergenceDivergence(Equity series, int emaPeriodCount1, int emaPeriodCount2, int demPeriodCount) 
            : base(series, emaPeriodCount1, emaPeriodCount2, demPeriodCount)
        {
            _emaIndicator1 = new ExponentialMovingAverage(series, emaPeriodCount1);
            _emaIndicator2 = new ExponentialMovingAverage(series, emaPeriodCount2);
        }

        private decimal SmoothingFactor => Convert.ToDecimal(2.0 / (DemPeriodCount + 1));

        public int EmaPeriodCount1 => Parameters[0];

        public int EmaPeriodCount2 => Parameters[1];

        public int DemPeriodCount => Parameters[2];

        protected override IAnalyticResult<decimal> ComputeResultByIndex(int index)
        {
            var tuple = (Dif: 0m, Dem: 0m, Osc: 0m);
            var diff = _emaIndicator1.ComputeByIndex(index).Ema
                - _emaIndicator2.ComputeByIndex(index).Ema;

            if (index == 0)
                tuple = (diff, diff, 0);
            else
            {
                var prevDem = GetComputed<IndicatorResult>(index - 1).Dem;
                var dem = prevDem + (SmoothingFactor * (diff - prevDem));
                tuple = (diff, dem, diff - dem);
            }

            var result = new IndicatorResult(Series[index].DateTime, tuple.Dif, tuple.Dem, tuple.Osc);
            CacheComputed(result);
            return result;
        }

        public IndicatorResultTimeSeries<IndicatorResult> Compute(DateTime? startTime = null, DateTime? endTime = null)
            => new IndicatorResultTimeSeries<IndicatorResult>(Series.Name, ComputeResults<IndicatorResult>(startTime, endTime), Series.Period, Series.MaxTickCount);

        public IndicatorResult ComputeByDateTime(DateTime dateTime)
            => ComputeResultByDateTime<IndicatorResult>(dateTime);

        public IndicatorResult ComputeByIndex(int index)
            => ComputeResultByIndex<IndicatorResult>(index);
    }
}
