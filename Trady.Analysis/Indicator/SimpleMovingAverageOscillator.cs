using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Infrastructure;
using Trady.Core;

namespace Trady.Analysis.Indicator
{
    public partial class SimpleMovingAverageOscillator : AnalyzableBase<decimal, decimal?>
    {
        private SimpleMovingAverage _sma1, _sma2;

        public SimpleMovingAverageOscillator(IList<Candle> candles, int periodCount1, int periodCount2)
            : this(candles.Select(c => c.Close).ToList(), periodCount1, periodCount2)
        {
        }

        public SimpleMovingAverageOscillator(IList<decimal> closes, int periodCount1, int periodCount2)
            : base(closes)
        {
            _sma1 = new SimpleMovingAverage(closes, periodCount1);
            _sma2 = new SimpleMovingAverage(closes, periodCount2);

            PeriodCount1 = periodCount1;
            PeriodCount2 = periodCount2;
        }

        public int PeriodCount1 { get; private set; }

        public int PeriodCount2 { get; private set; }

        protected override decimal? ComputeByIndexImpl(int index) => _sma1[index] - _sma2[index];
    }
}