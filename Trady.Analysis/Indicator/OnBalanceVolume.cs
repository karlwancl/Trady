using System;
using Trady.Core;

namespace Trady.Analysis.Indicator
{
    public partial class OnBalanceVolume : CachedIndicatorBase
    {
        private const string ObvTag = "Obv";

        public OnBalanceVolume(Equity equity) : base(equity, null)
        {
        }

        protected override IAnalyticResult<decimal> ComputeResultByIndex(int index)
        {
            decimal obv = 0;
            var candle = Equity[index];

            if (index == 0)
                obv = candle.Volume;
            else
            {
                var prevCandle = Equity[index - 1];
                var prevObv = GetComputed<IndicatorResult>(index - 1).Obv;
                long increment = candle.Volume * (candle.Close > prevCandle.Close ? 1 : (candle.Close == prevCandle.Close ? 0 : -1));
                obv = prevObv + increment;
            }

            var result = new IndicatorResult(candle.DateTime, obv);
            CacheComputed(result);
            return result;
        }

        public IndicatorResultTimeSeries<IndicatorResult> Compute(DateTime? startTime = null, DateTime? endTime = null)
            => new IndicatorResultTimeSeries<IndicatorResult>(Equity.Name, ComputeResults<IndicatorResult>(startTime, endTime), Equity.Period, Equity.MaxTickCount);

        public IndicatorResult ComputeByDateTime(DateTime dateTime)
            => ComputeResultByDateTime<IndicatorResult>(dateTime);

        public IndicatorResult ComputeByIndex(int index)
            => ComputeResultByIndex<IndicatorResult>(index);
    }
}
