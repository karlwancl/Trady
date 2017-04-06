using System.Collections.Generic;
using System.Linq;
using Trady.Core;

namespace Trady.Analysis.Indicator
{
    public partial class HighestHigh : IndicatorBase<decimal, decimal?>
    {
        public HighestHigh(IList<Candle> candles, int periodCount) :
            this(candles.Select(c => c.High).ToList(), periodCount)
        {
        }

        public HighestHigh(IList<decimal> highs, int periodCount) : base(highs, periodCount)
        {
        }

        public int PeriodCount => Parameters[0];

        protected override decimal? ComputeByIndexImpl(int index)
            => index >= PeriodCount - 1 ? Inputs.Skip(index - PeriodCount + 1).Take(PeriodCount).Max() : (decimal?)null;
    }
}