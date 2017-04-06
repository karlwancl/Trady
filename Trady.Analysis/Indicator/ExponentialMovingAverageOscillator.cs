using System.Collections.Generic;
using System.Linq;
using Trady.Core;

namespace Trady.Analysis.Indicator
{
    public partial class ExponentialMovingAverageOscillator : IndicatorBase<decimal, decimal?>
    {
        private ExponentialMovingAverage _ema1, _ema2;

        public ExponentialMovingAverageOscillator(IList<Candle> candles, int periodCount1, int periodCount2) :
            this(candles.Select(c => c.Close).ToList(), periodCount1, periodCount2)
        {
        }

        public ExponentialMovingAverageOscillator(IList<decimal> closes, int periodCount1, int periodCount2)
            : base(closes, periodCount1, periodCount2)
        {
            _ema1 = new ExponentialMovingAverage(closes, periodCount1);
            _ema2 = new ExponentialMovingAverage(closes, periodCount2);
        }

        public int PeriodCount1 => Parameters[0];

        public int PeriodCount2 => Parameters[1];

        protected override decimal? ComputeByIndexImpl(int index)
            => _ema1[index] - _ema2[index];
    }
}