using System;
using Trady.Core;
using static Trady.Analysis.Indicator.AccumulationDistributionLine;

namespace Trady.Analysis.Indicator
{
    public partial class AccumulationDistributionLine : CachedIndicatorBase<IndicatorResult>
    {
        public AccumulationDistributionLine(Equity equity) : base(equity, null)
        {
        }

        protected override Func<int, IndicatorResult> FirstValueFunction
            => i => new IndicatorResult(Equity[i].DateTime, Equity[i].Volume);

        protected override IndicatorResult ComputeByIndexUncached(int index)
        {
            var candle = Equity[index];            
            decimal? accumDist = 0;

            if (index == 0)
                accumDist = candle.Volume;
            else
            {
                var prevCandle = Equity[index - 1];
                var prevAccumDist = ComputeByIndex(index - 1).AccumDist;

                decimal ratio = (candle.High == candle.Low) ?
                    (candle.Close / prevCandle.Close) - 1 :
                    (candle.Close * 2 - candle.Low - candle.High) / (candle.High - candle.Low);

                accumDist = prevAccumDist + ratio * candle.Volume;
            }

            return new IndicatorResult(candle.DateTime, accumDist);
        }
    }
}
