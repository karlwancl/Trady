using System.Collections.Generic;
using System.Linq;
using Trady.Core;

namespace Trady.Analysis.Indicator
{
    public partial class ExponentialMovingAverage : IndicatorBase<decimal, decimal?>
    {
        private GenericExponentialMovingAverage<decimal> _ema;

        public ExponentialMovingAverage(IList<Candle> candles, int periodCount) :
            this(candles.Select(c => c.Close).ToList(), periodCount)
        {
        }

        public ExponentialMovingAverage(IList<decimal> closes, int periodCount) : base(closes, periodCount)
        {
            _ema = new GenericExponentialMovingAverage<decimal>(
                closes,
                0,
                i => Inputs[i],
                i => Inputs[i],
                i => 2.0m / (periodCount + 1));
        }

        public int PeriodCount => Parameters[0];

        protected override decimal? ComputeByIndexImpl(int index) => _ema[index];
    }
}