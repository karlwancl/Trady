using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Infrastructure;
using Trady.Core;

namespace Trady.Analysis.Indicator
{
    public class PlusDirectionalMovement : AnalyzableBase<decimal, decimal?>
    {
        public PlusDirectionalMovement(IList<Candle> candles)
            : this(candles.Select(c => c.High).ToList())
        {
        }

        public PlusDirectionalMovement(IList<decimal> highs) : base(highs)
        {
        }

        protected override decimal? ComputeByIndexImpl(int index)
            => index > 0 ? Inputs[index] - Inputs[index - 1] : (decimal?)null;
    }
}