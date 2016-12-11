using Trady.Analysis.Indicator;
using Trady.Core;

namespace Trady.Analysis.Pattern.Indicator
{
    public class MovingAverageConvergenceDivergenceCrossover : PatternBase<IsMatchedMultistateResult<Trend>>
    {
        private MovingAverageConvergenceDivergence _macdIndicator;

        public MovingAverageConvergenceDivergenceCrossover(Equity equity, int emaPeriodCount1, int emaPeriodCount2, int demPeriodCount) : base(equity)
        {
            _macdIndicator = new MovingAverageConvergenceDivergence(equity, emaPeriodCount1, emaPeriodCount2, demPeriodCount);
        }

        protected override TickBase ComputeResultByIndex(int index)
        {
            if (index < 1)
                return new IsMatchedMultistateResult<Trend>(Equity[index].DateTime, false, Trend.NonTrended);

            var latest = _macdIndicator.ComputeByIndex(index);
            var secondLatest = _macdIndicator.ComputeByIndex(index - 1);
            return new IsMatchedMultistateResult<Trend>(Equity[index].DateTime, latest.Osc * secondLatest.Osc < 0, GetTrend(latest.Osc));
        }

        private Trend GetTrend(decimal value)
        {
            if (value > 0) return Trend.Bullish;
            if (value < 0) return Trend.Bearish;
            return Trend.NonTrended;
        }
    }
}
