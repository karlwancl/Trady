using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Infrastructure;

namespace Trady.Analysis.Pattern.Indicator
{
    public class IsHighestPrice : AnalyzableBase<decimal, bool?>
    {
        private int _periodCount;

        public IsHighestPrice(IList<Core.Candle> candles, int periodCount)
            : this(candles.Select(c => c.Close).ToList(), periodCount)
        {
        }

        public IsHighestPrice(IList<decimal> closes, int periodCount)
            : base(closes)
        {
            _periodCount = periodCount;
        }

        protected override bool? ComputeByIndexImpl(int index)
            => Inputs.Skip(Inputs.Count - _periodCount).Max() == Inputs[index];
    }
}