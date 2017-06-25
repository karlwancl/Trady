using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Infrastructure;
using Trady.Core;

namespace Trady.Analysis.Indicator
{
    public class MinusDirectionalMovement : AnalyzableBase<decimal, decimal?>
    {
        public MinusDirectionalMovement(IList<Candle> candles)
            : this(candles.Select(c => c.Low).ToList())
        {
        }

        public MinusDirectionalMovement(IList<decimal> lows) : base(lows)
        {
        }

        protected override decimal? ComputeByIndexImpl(int index)
            => index > 0 ? Inputs[index - 1] - Inputs[index] : (decimal?)null;
    }
}