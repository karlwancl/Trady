using System;
using Trady.Core;

namespace Trady.Analysis.Indicator
{
    public partial class ExponentialMovingAverage : CachedIndicatorBase
    {
        private const string EmaTag = "Ema";

        public ExponentialMovingAverage(Equity series, int periodCount) : base(series, periodCount)
        {
        }

        public int PeriodCount => Parameters[0];

        private decimal SmoothingFactor => Convert.ToDecimal(2.0 / (PeriodCount + 1));

        protected override IAnalyticResult<decimal> ComputeResultByIndex(int index)
        {
            decimal ema = 0;
            var candle = Series[index];

            if (index == 0)
                ema = candle.Close;
            else
            {
                var prevEma = GetComputed<IndicatorResult>(index - 1).Ema;
                ema = prevEma + (SmoothingFactor * (candle.Close - prevEma));
            }

            var result = new IndicatorResult(candle.DateTime, ema);
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
