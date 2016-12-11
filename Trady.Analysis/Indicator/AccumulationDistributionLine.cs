using System;
using Trady.Core;

namespace Trady.Analysis.Indicator
{
    public partial class AccumulationDistributionLine : CachedIndicatorBase
    {
        public AccumulationDistributionLine(Equity equity) : base(equity, null)
        {
        }

        protected override TickBase ComputeResultByIndex(int index)
        {
            decimal accumDist = 0;
            var candle = Equity[index];

            if (index == 0)
                accumDist = candle.Volume;
            else
            {
                var prevCandle = Equity[index - 1];
                var prevAccumDist = GetComputed<IndicatorResult>(index - 1).AccumDist;

                decimal ratio = (candle.High == candle.Low) ?
                    (candle.Close / prevCandle.Close) - 1 :
                    (candle.Close * 2 - candle.Low - candle.High) / (candle.High - candle.Low);

                accumDist = prevAccumDist + ratio * candle.Volume;
            }

            var result = new IndicatorResult(candle.DateTime, accumDist);
            CacheComputed(result);
            return result;
        }

        public TimeSeries<IndicatorResult> Compute(DateTime? startTime = null, DateTime? endTime = null)
            => new TimeSeries<IndicatorResult>(Equity.Name, ComputeResults<IndicatorResult>(startTime, endTime), Equity.Period, Equity.MaxTickCount);

        public IndicatorResult ComputeByDateTime(DateTime dateTime)
            => ComputeResultByDateTime<IndicatorResult>(dateTime);

        public IndicatorResult ComputeByIndex(int index)
            => ComputeResultByIndex<IndicatorResult>(index);
    }
}
