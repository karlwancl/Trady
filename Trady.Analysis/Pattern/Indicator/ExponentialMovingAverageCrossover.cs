using Trady.Analysis.Indicator;
using Trady.Analysis.Pattern.Helper;
using Trady.Core;

namespace Trady.Analysis.Pattern.Indicator
{
    public class ExponentialMovingAverageCrossover : IndicatorBase<PatternResult<Crossover?>>
    {
        private ExponentialMovingAverageOscillator _emaOscillator;

        public ExponentialMovingAverageCrossover(Equity equity, int periodCount1, int periodCount2)
            : base(equity, periodCount1, periodCount2)
        {
            _emaOscillator = new ExponentialMovingAverageOscillator(equity, periodCount1, periodCount2);
        }

        protected override PatternResult<Crossover?> ComputeByIndexImpl(int index)
        {
            if (index < 1)
                return new PatternResult<Crossover?>(Equity[index].DateTime, null);

            var latest = _emaOscillator.ComputeByIndex(index);
            var secondLatest = _emaOscillator.ComputeByIndex(index - 1);

            return new PatternResult<Crossover?>(Equity[index].DateTime, Decision.IsCrossover(latest.Osc, secondLatest.Osc));
        }
    }
}