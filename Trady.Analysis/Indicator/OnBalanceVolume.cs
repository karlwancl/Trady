using System;
using Trady.Core;
using static Trady.Analysis.Indicator.OnBalanceVolume;

namespace Trady.Analysis.Indicator
{
    public partial class OnBalanceVolume : CachedIndicatorBase<IndicatorResult>
    {
        public OnBalanceVolume(Equity equity) : base(equity, null)
        {
        }

        protected override Func<int, IndicatorResult> FirstValueFunction
            => i => new IndicatorResult(Equity[i].DateTime, Equity[i].Volume);

        protected override IndicatorResult ComputeByIndexUncached(int index)
        {
            long? obv = 0;
            var candle = Equity[index];

            if (index == 0)
                obv = candle.Volume;
            else
            {
                var prevCandle = Equity[index - 1];
                var prevObv = ComputeByIndex(index - 1).Obv;
                long increment = candle.Volume * (candle.Close > prevCandle.Close ? 1 : (candle.Close == prevCandle.Close ? 0 : -1));
                obv = prevObv + increment;
            }

            return new IndicatorResult(candle.DateTime, obv);
        }
    }
}
