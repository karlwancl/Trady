using Trady.Analysis.Indicator;
using Trady.Analysis.Pattern.Helper;
using Trady.Core;

namespace Trady.Analysis.Pattern.Indicator
{
    public partial class StochasticsCrossover
    {
        public abstract class IndicatorBase : IndicatorBase<PatternResult<Crossover?>>
        {
            private IndicatorBase<Stochastics.IndicatorResult> _stoIndicator;

            protected IndicatorBase(Equity equity, IndicatorBase<Stochastics.IndicatorResult> stoIndicator)
                : base(equity, stoIndicator.Parameters)
            {
                _stoIndicator = stoIndicator;
            }

            protected override PatternResult<Crossover?> ComputeByIndexImpl(int index)
            {
                if (index < 1)
                    return new PatternResult<Crossover?>(Equity[index].DateTime, null);

                var latest = _stoIndicator.ComputeByIndex(index);
                var secondLatest = _stoIndicator.ComputeByIndex(index - 1);

                var latestKdOsc = latest.K - latest.D;
                var secondLatestKsOsc = secondLatest.K - secondLatest.D;

                return new PatternResult<Crossover?>(Equity[index].DateTime, Decision.IsCrossover(latestKdOsc, secondLatestKsOsc));
            }
        }
    }
}