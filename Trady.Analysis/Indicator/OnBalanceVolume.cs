using System;
using Trady.Core;
using static Trady.Analysis.Indicator.OnBalanceVolume;

namespace Trady.Analysis.Indicator
{
    public partial class OnBalanceVolume : CacheIndicatorBase<IndicatorResult>
    {
        public OnBalanceVolume(Equity equity) : base(equity, null)
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
            long increment = candle.Volume * (candle.Close > prevCandle.Close ? 1 : (candle.Close == prevCandle.Close ? 0 : -1));
            return new IndicatorResult(candle.DateTime, prevTick.Obv + increment);
        }
    }
}
