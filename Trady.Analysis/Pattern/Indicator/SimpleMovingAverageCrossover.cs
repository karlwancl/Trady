using Trady.Analysis.Indicator;
using Trady.Analysis.Pattern.Helper;
using Trady.Core;

namespace Trady.Analysis.Pattern.Indicator
{
    public class SimpleMovingAverageCrossover : IndicatorBase<PatternResult<Crossover?>>
    {
        private SimpleMovingAverageOscillator _smaOscillator;

        public SimpleMovingAverageCrossover(Equity equity, int periodCount1, int periodCount2)
            : base(equity, periodCount1, periodCount2)
        {
            _smaOscillator = new SimpleMovingAverageOscillator(equity, periodCount1, periodCount2);
        }

        protected override PatternResult<Crossover?> ComputeByIndexImpl(int index)
        {
            if (index < 1)
                return new PatternResult<Crossover?>(Equity[index].DateTime, null);

            var latest = _smaOscillator.ComputeByIndex(index);
            var secondLatest = _smaOscillator.ComputeByIndex(index - 1);

            return new PatternResult<Crossover?>(Equity[index].DateTime, Decision.IsCrossover(latest.Osc, secondLatest.Osc));
        }
    }
}