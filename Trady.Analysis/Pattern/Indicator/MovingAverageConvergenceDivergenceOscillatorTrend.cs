using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Indicator;
using Trady.Analysis.Pattern.State;

namespace Trady.Analysis.Pattern.Indicator
{
    public class MovingAverageConvergenceDivergenceOscillatorTrend : IndicatorBase<decimal, Trend?>
    {
        private MovingAverageConvergenceDivergence _macd;

        public MovingAverageConvergenceDivergenceOscillatorTrend(IList<Core.Candle> candles, int emaPeriodCount1, int emaPeriodCount2, int demPeriodCount)
            : this(candles.Select(c => c.Close).ToList(), emaPeriodCount1, emaPeriodCount2, demPeriodCount)
        {
        }

        public MovingAverageConvergenceDivergenceOscillatorTrend(IList<decimal> closes, int emaPeriodCount1, int emaPeriodCount2, int demPeriodCount)
            : base(closes, emaPeriodCount1, emaPeriodCount2, demPeriodCount)
        {
            _macd = new MovingAverageConvergenceDivergence(closes, emaPeriodCount1, emaPeriodCount2, demPeriodCount);
        }

        protected override Trend? ComputeByIndexImpl(int index)
            => index >= 1 ? StateHelper.IsTrending(_macd[index].MacdHistogram - _macd[index - 1].MacdHistogram) : null;
    }
}