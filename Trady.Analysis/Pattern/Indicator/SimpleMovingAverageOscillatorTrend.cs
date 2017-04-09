using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Indicator;
using Trady.Analysis.Infrastructure;
using Trady.Analysis.Pattern.State;

namespace Trady.Analysis.Pattern.Indicator
{
    public class SimpleMovingAverageOscillatorTrend : AnalyzableBase<decimal, Trend?>
    {
        private SimpleMovingAverageOscillator _smaOsc;

        public SimpleMovingAverageOscillatorTrend(IList<Core.Candle> candles, int periodCount1, int periodCount2)
            : this(candles.Select(c => c.Close).ToList(), periodCount1, periodCount2)
        {
        }

        public SimpleMovingAverageOscillatorTrend(IList<decimal> closes, int periodCount1, int periodCount2)
            : base(closes)
        {
            _smaOsc = new SimpleMovingAverageOscillator(closes, periodCount1, periodCount2);
        }

        protected override Trend? ComputeByIndexImpl(int index)
            => index >= 1 ? StateHelper.IsTrending(_smaOsc[index] - _smaOsc[index - 1]) : null;
    }
}