using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Indicator;
using Trady.Analysis.Infrastructure;
using Trady.Analysis.Pattern.State;

namespace Trady.Analysis.Pattern.Indicator
{
    public class MovingAverageConvergenceDivergenceCrossover : AnalyzableBase<decimal, Crossover?>
    {
        private MovingAverageConvergenceDivergence _macd;

        public MovingAverageConvergenceDivergenceCrossover(IList<Core.Candle> candles, int emaPeriodCount1, int emaPeriodCount2, int demPeriodCount)
            : this(candles.Select(c => c.Close).ToList(), emaPeriodCount1, emaPeriodCount2, demPeriodCount)
        {
        }

        public MovingAverageConvergenceDivergenceCrossover(IList<decimal> closes, int emaPeriodCount1, int emaPeriodCount2, int demPeriodCount)
            : base(closes)
        {
            _macd = new MovingAverageConvergenceDivergence(closes, emaPeriodCount1, emaPeriodCount2, demPeriodCount);
        }

        protected override Crossover? ComputeByIndexImpl(int index)
            => index >= 1 ? StateHelper.IsCrossover(_macd[index].MacdHistogram, _macd[index - 1].MacdHistogram) : null;
    }
}