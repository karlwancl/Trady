using System;
using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Infrastructure;
using Trady.Core;

namespace Trady.Analysis.Indicator
{
    public partial class EfficiencyRatio : AnalyzableBase<decimal, decimal?>
    {
        public EfficiencyRatio(IList<Candle> candles, int periodCount) :
            this(candles.Select(c => c.Close).ToList(), periodCount)
        {
        }

        public EfficiencyRatio(IList<decimal> closes, int periodCount) : base(closes)
        {
            PeriodCount = periodCount;
        }

        public int PeriodCount { get; private set; }

        protected override decimal? ComputeByIndexImpl(int index)
        {
            if (index <= 0 || index < PeriodCount)
                return null;

            decimal? change = Math.Abs(Inputs[index] - Inputs[index - PeriodCount]);
            decimal? volatility = Enumerable.Range(index - PeriodCount + 1, PeriodCount).Select(i => Math.Abs(Inputs[i] - Inputs[i - 1])).Sum();
            return change / volatility;
        }
    }
}