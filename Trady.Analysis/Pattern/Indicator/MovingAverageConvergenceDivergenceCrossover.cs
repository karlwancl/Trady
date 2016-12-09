using Trady.Analysis.Indicator;
using Trady.Core;

namespace Trady.Analysis.Pattern.Indicator
{
    public class MovingAverageConvergenceDivergenceCrossover : PatternBase<DirectionalPatternResult>
    {
        private MovingAverageConvergenceDivergence _macdIndicator;

        public MovingAverageConvergenceDivergenceCrossover(Equity series, int emaPeriodCount1, int emaPeriodCount2, int demPeriodCount) : base(series)
        {
            _macdIndicator = new MovingAverageConvergenceDivergence(series, emaPeriodCount1, emaPeriodCount2, demPeriodCount);
        }

        protected override IAnalyticResult<bool> ComputeResultByIndex(int index)
        {
            if (index < 1)
                return new DirectionalPatternResult(Series[index].DateTime, false, false, false);

            var latest = _macdIndicator.ComputeByIndex(index);
            var secondLatest = _macdIndicator.ComputeByIndex(index - 1);
            return new DirectionalPatternResult(Series[index].DateTime, latest.Osc * secondLatest.Osc < 0, latest.Osc > 0, latest.Osc < 0);
        }
    }
}
