using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Infrastructure;
using Trady.Core;

namespace Trady.Analysis.Indicator
{
    public partial class ClosePricePercentageChange : AnalyzableBase<decimal, decimal?>
    {
        public ClosePricePercentageChange(IList<Candle> candles)
            : this(candles.Select(c => c.Close).ToList())
        {
        }

        public ClosePricePercentageChange(IList<decimal> closes) : base(closes)
        {
        }

        protected override decimal? ComputeByIndexImpl(int index)
           => index > 0 ? (Inputs[index] - Inputs[index - 1]) / Inputs[index - 1] * 100 : (decimal?)null;
    }
}