using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Infrastructure;
using Trady.Core;

namespace Trady.Analysis.Indicator
{
    public partial class ModifiedExponentialMovingAverage : AnalyzableBase<decimal, decimal?>
    {
        private GenericExponentialMovingAverage<decimal> _gema;

        public ModifiedExponentialMovingAverage(IList<Candle> candles, int periodCount)
            : this(candles.Select(c => c.Close).ToList(), periodCount)
        {
        }

        public ModifiedExponentialMovingAverage(IList<decimal> closes, int periodCount) : base(closes)
        {
            _gema = new GenericExponentialMovingAverage<decimal>(
                closes,
                0,
                i => Inputs[i],
                i => Inputs[i],
                i => 1.0m / periodCount);

            PeriodCount = periodCount;
        }

        public int PeriodCount { get; private set; }

        protected override decimal? ComputeByIndexImpl(int index) => _gema[index];
    }
}