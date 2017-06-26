using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Infrastructure;
using Trady.Core;

namespace Trady.Analysis.Indicator
{
    public partial class ClosePriceChange : AnalyzableBase<decimal, decimal?>
    {
        public int NumberOfDays { get; }

        public ClosePriceChange(IList<Candle> candles, int numberOfDays = 1)
            : this(candles.Select(c => c.Close).ToList(), numberOfDays)
        {
        }

        public ClosePriceChange(IList<decimal> closes, int numberOfDays = 1) : base(closes)
        {
            NumberOfDays = numberOfDays;
        }

        protected override decimal? ComputeByIndexImpl(int index)
           => index >= NumberOfDays ? Inputs[index] - Inputs[index - NumberOfDays] : (decimal?)null;
    }
}