using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Indicator;
using Trady.Analysis.Pattern.State;

namespace Trady.Analysis.Pattern.Indicator
{
    public class IsAboveExponentialMovingAverage : IndicatorBase<decimal, Match?>
    {
        private ExponentialMovingAverage _ema;

        public IsAboveExponentialMovingAverage(IList<Core.Candle> candles, int periodCount)
            : this(candles.Select(c => c.Close).ToList(), periodCount)
        {
        }

        public IsAboveExponentialMovingAverage(IList<decimal> closes, int periodCount)
            : base(closes, periodCount)
        {
            _ema = new ExponentialMovingAverage(closes, periodCount);
        }

        protected override Match? ComputeByIndexImpl(int index)
            => StateHelper.IsMatch(Inputs[index].IsLargerThan(_ema[index]));
    }
}