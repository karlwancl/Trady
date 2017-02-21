using Trady.Core;
using static Trady.Analysis.Indicator.AccumulationDistributionLine;

namespace Trady.Analysis.Indicator
{
    public partial class AccumulationDistributionLine : CacheIndicatorBase<IndicatorResult>
    {
        public AccumulationDistributionLine(Equity equity) : base(equity, null)
        {
        }

        protected override int FirstValueIndex => 0;

        protected override IndicatorResult ComputeNullValue(int index)
            => new IndicatorResult(Equity[index].DateTime, null);

        protected override IndicatorResult ComputeFirstValue(int index)
            => new IndicatorResult(Equity[index].DateTime, Equity[index].Volume);

        protected override IndicatorResult ComputeIndexValue(int index, IndicatorResult prevTick)
        {
            var candle = Equity[index];
            var prevCandle = Equity[index - 1];

            decimal ratio = (candle.High == candle.Low) ?
                (candle.Close / prevCandle.Close) - 1 :
                (candle.Close * 2 - candle.Low - candle.High) / (candle.High - candle.Low);

            return new IndicatorResult(candle.DateTime, prevTick.AccumDist + ratio * candle.Volume);
        }
    }
}