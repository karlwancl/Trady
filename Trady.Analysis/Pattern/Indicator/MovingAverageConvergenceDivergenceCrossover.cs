using Trady.Analysis.Indicator;
using Trady.Analysis.Pattern.Helper;
using Trady.Core;

namespace Trady.Analysis.Pattern.Indicator
{
    public class MovingAverageConvergenceDivergenceCrossover : IndicatorBase<PatternResult<Crossover?>>
    {
        private MovingAverageConvergenceDivergence _macdIndicator;

        public MovingAverageConvergenceDivergenceCrossover(Equity equity, int emaPeriodCount1, int emaPeriodCount2, int demPeriodCount)
            : base(equity, emaPeriodCount1, emaPeriodCount2, demPeriodCount)
        {
            _macdIndicator = new MovingAverageConvergenceDivergence(equity, emaPeriodCount1, emaPeriodCount2, demPeriodCount);
        }

        protected override PatternResult<Crossover?> ComputeByIndexImpl(int index)
        {
            if (index < 1)
                return new PatternResult<Crossover?>(Equity[index].DateTime, null);

            var latest = _macdIndicator.ComputeByIndex(index);
            var secondLatest = _macdIndicator.ComputeByIndex(index - 1);

            return new PatternResult<Crossover?>(Equity[index].DateTime, Decision.IsCrossover(latest.Osc, secondLatest.Osc));
        }
    }
}