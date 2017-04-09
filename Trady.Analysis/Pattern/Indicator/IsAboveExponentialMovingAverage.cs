using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Indicator;
using Trady.Analysis.Infrastructure;
using Trady.Analysis.Pattern.State;

namespace Trady.Analysis.Pattern.Indicator
{
    public class IsAboveExponentialMovingAverage : AnalyzableBase<decimal, bool?>
    {
        private ExponentialMovingAverage _ema;

        public IsAboveExponentialMovingAverage(IList<Core.Candle> candles, int periodCount)
            : this(candles.Select(c => c.Close).ToList(), periodCount)
        {
        }

        public IsAboveExponentialMovingAverage(IList<decimal> closes, int periodCount)
            : base(closes)
        {
            _ema = new ExponentialMovingAverage(closes, periodCount);
        }

        protected override bool? ComputeByIndexImpl(int index)
            => Inputs[index].IsLargerThan(_ema[index]);
    }
}