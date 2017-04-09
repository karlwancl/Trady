using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Infrastructure;
using Trady.Core;

namespace Trady.Analysis.Indicator
{
    public partial class ExponentialMovingAverage : AnalyzableBase<decimal, decimal?>
    {
        private GenericExponentialMovingAverage<decimal> _ema;

        public ExponentialMovingAverage(IList<Candle> candles, int periodCount) :
            this(candles.Select(c => c.Close).ToList(), periodCount)
        {
        }

        public ExponentialMovingAverage(IList<decimal> closes, int periodCount) : base(closes)
        {
            _ema = new GenericExponentialMovingAverage<decimal>(
                closes,
                0,
                i => Inputs[i],
                i => Inputs[i],
                i => 2.0m / (periodCount + 1));

            PeriodCount = periodCount;
        }

        public int PeriodCount { get; private set; }

        protected override decimal? ComputeByIndexImpl(int index) => _ema[index];
    }
}