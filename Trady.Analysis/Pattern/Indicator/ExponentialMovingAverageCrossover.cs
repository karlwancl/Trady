using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Indicator;
using Trady.Analysis.Infrastructure;
using Trady.Analysis.Pattern.State;

namespace Trady.Analysis.Pattern.Indicator
{
    public class ExponentialMovingAverageCrossover : AnalyzableBase<decimal, Crossover?>
    {
        private ExponentialMovingAverageOscillator _emaOsc;

        public ExponentialMovingAverageCrossover(IList<Core.Candle> candles, int periodCount1, int periodCount2)
            : this(candles.Select(c => c.Close).ToList(), periodCount1, periodCount2)
        {
        }

        public ExponentialMovingAverageCrossover(IList<decimal> closes, int periodCount1, int periodCount2)
            : base(closes)
        {
            _emaOsc = new ExponentialMovingAverageOscillator(closes, periodCount1, periodCount2);
        }

        protected override Crossover? ComputeByIndexImpl(int index)
            => index >= 1 ? StateHelper.IsCrossover(_emaOsc[index], _emaOsc[index - 1]) : null;
    }
}