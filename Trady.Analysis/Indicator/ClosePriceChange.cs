using System.Collections.Generic;
using System.Linq;
using Trady.Core;

namespace Trady.Analysis.Indicator
{
    public partial class ClosePriceChange : IndicatorBase<decimal, decimal?>
    {
        public ClosePriceChange(IList<Candle> candles)
            : this(candles.Select(c => c.Close).ToList())
        {
        }

        public ClosePriceChange(IList<decimal> closes) : base(closes)
        {
        }

        protected override decimal? ComputeByIndexImpl(int index)
           => index > 0 ? Inputs[index] - Inputs[index - 1] : (decimal?)null;
    }
}