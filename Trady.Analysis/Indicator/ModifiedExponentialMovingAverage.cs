using System.Collections.Generic;
using System.Linq;
using Trady.Core;

namespace Trady.Analysis.Indicator
{
    public partial class ModifiedExponentialMovingAverage : IndicatorBase<decimal, decimal?>
    {
        private GenericExponentialMovingAverage<decimal> _gema;

        public ModifiedExponentialMovingAverage(IList<Candle> candles, int periodCount)
            : this(candles.Select(c => c.Close).ToList(), periodCount)
        {
        }

        public ModifiedExponentialMovingAverage(IList<decimal> closes, int periodCount) : base(closes, periodCount)
        {
            _gema = new GenericExponentialMovingAverage<decimal>(
                closes,
                0,
                i => Inputs[i],
                i => Inputs[i],
                i => 1.0m / periodCount);
        }

        public int PeriodCount => Parameters[0];

        protected override decimal? ComputeByIndexImpl(int index) => _gema[index];
    }
}