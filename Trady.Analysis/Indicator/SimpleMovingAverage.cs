using System;
using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Helper;
using Trady.Analysis.Infrastructure;
using Trady.Core;

namespace Trady.Analysis.Indicator
{
    public partial class SimpleMovingAverage : AnalyzableBase<decimal, decimal?>
    {
        public SimpleMovingAverage(IList<Candle> candles, int periodCount)
            : this(candles.Select(c => c.Close).ToList(), periodCount)
        {
        }

        public SimpleMovingAverage(IList<decimal> closes, int periodCount) : base(closes)
        {
            PeriodCount = periodCount;
        }

        public int PeriodCount { get; private set; }

        protected override decimal? ComputeByIndexImpl(int index) => Inputs.Avg(PeriodCount, index);
    }

    public class SimpleMovingAverageWrapper : AnalyzableWrapperBase<SimpleMovingAverage, decimal, decimal?>
    {
        public SimpleMovingAverageWrapper(IList<Candle> candles, int periodCount) : base(candles, periodCount)
        {
        }

        protected override Func<Candle, decimal> MappingFunction => c => c.Close;
    }
}