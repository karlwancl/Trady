using System;
using Trady.Core;

namespace Trady.Analysis.Indicator
{
    public partial class AccumulationDistributionLine : CachedIndicatorBase
    {
        private const string AccumDistTag = "AccumDist";

        public AccumulationDistributionLine(Equity series) : base(series, null)
        {
        }

        protected override IAnalyticResult<decimal> ComputeResultByIndex(int index)
        {
            decimal accumDist = 0;
            var candle = Series[index];

            if (index == 0)
                accumDist = candle.Volume;
            else
            {
                var prevCandle = Series[index - 1];
                var prevAccumDist = GetComputed<IndicatorResult>(index - 1).AccumDist;

                decimal ratio = (candle.Close / prevCandle.Close) - 1;
                if (candle.High != candle.Low)
                    ratio = (candle.Close * 2 - candle.Low - candle.High) / (candle.High - candle.Low);

                accumDist = prevAccumDist + ratio * candle.Volume;
            }

            var result = new IndicatorResult(candle.DateTime, accumDist);
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
