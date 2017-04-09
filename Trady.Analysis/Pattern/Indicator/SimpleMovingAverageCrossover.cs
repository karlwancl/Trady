using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Indicator;
using Trady.Analysis.Infrastructure;
using Trady.Analysis.Pattern.State;

namespace Trady.Analysis.Pattern.Indicator
{
    public class SimpleMovingAverageCrossover : AnalyzableBase<decimal, Crossover?>
    {
        private SimpleMovingAverageOscillator _smaOsc;

        public SimpleMovingAverageCrossover(IList<Core.Candle> candles, int periodCount1, int periodCount2)
            : this(candles.Select(c => c.Close).ToList(), periodCount1, periodCount2)
        {
        }

        public SimpleMovingAverageCrossover(IList<decimal> closes, int periodCount1, int periodCount2)
            : base(closes)
        {
            _smaOsc = new SimpleMovingAverageOscillator(closes, periodCount1, periodCount2);
        }

        protected override Crossover? ComputeByIndexImpl(int index)
            => index >= 1 ? StateHelper.IsCrossover(_smaOsc[index], _smaOsc[index - 1]) : null;
    }
}