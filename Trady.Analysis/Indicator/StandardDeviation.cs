using System;
using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Helper;
using Trady.Analysis.Infrastructure;
using Trady.Core;

namespace Trady.Analysis.Indicator
{
    public partial class StandardDeviation : AnalyzableBase<decimal, decimal?>
    {
        public StandardDeviation(IList<Candle> candles, int periodCount)
            : this(candles.Select(c => c.Close).ToList(), periodCount)
        {
        }

        public StandardDeviation(IList<decimal> closes, int periodCount) : base(closes)
        {
            PeriodCount = periodCount;
        }

        public int PeriodCount { get; private set; }

        protected override decimal? ComputeByIndexImpl(int index) => Inputs.Sd(PeriodCount, index);
    }
}