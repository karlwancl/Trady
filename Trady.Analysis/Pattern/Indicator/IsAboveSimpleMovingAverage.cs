using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Indicator;
using Trady.Analysis.Infrastructure;
using Trady.Analysis.Pattern.State;

namespace Trady.Analysis.Pattern.Indicator
{
    public class IsAboveSimpleMovingAverage : AnalyzableBase<decimal, bool?>
    {
        private SimpleMovingAverage _sma;

        public IsAboveSimpleMovingAverage(IList<Core.Candle> candles, int periodCount)
            : this(candles.Select(c => c.Close).ToList(), periodCount)
        {
        }

        public IsAboveSimpleMovingAverage(IList<decimal> closes, int periodCount)
            : base(closes)
        {
            _sma = new SimpleMovingAverage(closes, periodCount);
        }

        protected override bool? ComputeByIndexImpl(int index)
            => Inputs[index].IsLargerThan(_sma[index]);
    }
}