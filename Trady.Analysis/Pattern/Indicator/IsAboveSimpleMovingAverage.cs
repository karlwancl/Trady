using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Indicator;
using Trady.Analysis.Pattern.State;

namespace Trady.Analysis.Pattern.Indicator
{
    public class IsAboveSimpleMovingAverage : IndicatorBase<decimal, Match?>
    {
        private SimpleMovingAverage _sma;

        public IsAboveSimpleMovingAverage(IList<Core.Candle> candles, int periodCount)
            : this(candles.Select(c => c.Close).ToList(), periodCount)
        {
        }

        public IsAboveSimpleMovingAverage(IList<decimal> closes, int periodCount)
            : base(closes, periodCount)
        {
            _sma = new SimpleMovingAverage(closes, periodCount);
        }

        protected override Match? ComputeByIndexImpl(int index)
            => StateHelper.IsMatch(Inputs[index].IsLargerThan(_sma[index]));
    }
}