using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Indicator;
using Trady.Analysis.Infrastructure;
using Trady.Analysis.Pattern.State;

namespace Trady.Analysis.Pattern.Indicator
{
    public class IsLowestPrice : AnalyzableBase<decimal, bool?>
    {
        private int _periodCount;

        public IsLowestPrice(IList<Core.Candle> candles, int periodCount)
            : this(candles.Select(c => c.Close).ToList(), periodCount)
        {
        }

        public IsLowestPrice(IList<decimal> closes, int periodCount)
            : base(closes)
        {
            _periodCount = periodCount;
        }

        protected override bool? ComputeByIndexImpl(int index)
            => Inputs.Skip(Inputs.Count - _periodCount).Min() == Inputs[index];
    }
}