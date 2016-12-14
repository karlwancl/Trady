using Trady.Analysis.Indicator;
using Trady.Analysis.Pattern.Helper;
using Trady.Core;

namespace Trady.Analysis.Pattern.Indicator
{
    public partial class StochasticsCrossover
    {
        public abstract class IndicatorBase : IndicatorBase<IsMatchedMultistateResult<Trend?>>
        {
            private IndicatorBase<Stochastics.IndicatorResult> _stoIndicator;

            protected IndicatorBase(Equity equity, IndicatorBase<Stochastics.IndicatorResult> stoIndicator) 
                : base(equity, stoIndicator.Parameters)
            {
                _stoIndicator = stoIndicator;
            }

            public override IsMatchedMultistateResult<Trend?> ComputeByIndex(int index)
            {
                if (index < 1)
                    return new IsMatchedMultistateResult<Trend?>(Equity[index].DateTime, null, null);

                var latest = _stoIndicator.ComputeByIndex(index);
                var secondLatest = _stoIndicator.ComputeByIndex(index - 1);

                var latestKdOsc = latest.K - latest.D;
                var secondLatestKsOsc = secondLatest.K - secondLatest.D;

                return new IsMatchedMultistateResult<Trend?>(Equity[index].DateTime, 
                    Decision.IsCrossOver(latestKdOsc, secondLatestKsOsc), Decision.IsTrending(latestKdOsc));
            }
        }
    }
}
