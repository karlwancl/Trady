using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Infrastructure;
using Trady.Core;

namespace Trady.Analysis.Indicator
{
    public partial class HighestHigh : AnalyzableBase<decimal, decimal?>
    {
        public HighestHigh(IList<Candle> candles, int periodCount) :
            this(candles.Select(c => c.High).ToList(), periodCount)
        {
        }

        public HighestHigh(IList<decimal> highs, int periodCount) : base(highs)
        {
            PeriodCount = periodCount;
        }

        public int PeriodCount { get; private set; }

        protected override decimal? ComputeByIndexImpl(int index)
            => index >= PeriodCount - 1 ? Inputs.Skip(index - PeriodCount + 1).Take(PeriodCount).Max() : (decimal?)null;
    }
}