using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Infrastructure;
using Trady.Core;

namespace Trady.Analysis.Indicator
{
    public partial class ExponentialMovingAverageOscillator : AnalyzableBase<decimal, decimal?>
    {
        private ExponentialMovingAverage _ema1, _ema2;

        public ExponentialMovingAverageOscillator(IList<Candle> candles, int periodCount1, int periodCount2) :
            this(candles.Select(c => c.Close).ToList(), periodCount1, periodCount2)
        {
        }

        public ExponentialMovingAverageOscillator(IList<decimal> closes, int periodCount1, int periodCount2)
            : base(closes)
        {
            _ema1 = new ExponentialMovingAverage(closes, periodCount1);
            _ema2 = new ExponentialMovingAverage(closes, periodCount2);

            PeriodCount1 = periodCount1;
            PeriodCount2 = periodCount2;
        }

        public int PeriodCount1 { get; private set; }

        public int PeriodCount2 { get; private set; }

        protected override decimal? ComputeByIndexImpl(int index)
            => _ema1[index] - _ema2[index];
    }
}