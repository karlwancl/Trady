using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Indicator;
using Trady.Analysis.Infrastructure;
using Trady.Analysis.Pattern.State;

namespace Trady.Analysis.Pattern.Indicator
{
    public class ExponentialMovingAverageOscillatorTrend : AnalyzableBase<decimal, Trend?>
    {
        private ExponentialMovingAverageOscillator _emaOsc;

        public ExponentialMovingAverageOscillatorTrend(IList<Core.Candle> candles, int periodCount1, int periodCount2)
            : this(candles.Select(c => c.Close).ToList(), periodCount1, periodCount2)
        {
        }

        public ExponentialMovingAverageOscillatorTrend(IList<decimal> closes, int periodCount1, int periodCount2)
            : base(closes)
        {
            _emaOsc = new ExponentialMovingAverageOscillator(closes, periodCount1, periodCount2);
        }

        protected override Trend? ComputeByIndexImpl(int index)
            => index >= 1 ? StateHelper.IsTrending(_emaOsc[index] - _emaOsc[index - 1]) : null;
    }
}