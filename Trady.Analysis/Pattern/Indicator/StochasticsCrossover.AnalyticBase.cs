using Trady.Analysis.Indicator;
using Trady.Analysis.Pattern.Helper;
using Trady.Core;

namespace Trady.Analysis.Pattern.Indicator
{
    public partial class StochasticsCrossover
    {
        public abstract class AnalyticBase : AnalyticBase<IsMatchedMultistateResult<Trend?>>
        {
            private IndicatorBase<Stochastics.IndicatorResult> _stoIndicator;

            protected AnalyticBase(Equity equity, IndicatorBase<Stochastics.IndicatorResult> stoIndicator) : base(equity)
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
                    ResultExt.IsCrossOver(latestKdOsc, secondLatestKsOsc), ResultExt.IsTrending(latestKdOsc));
            }
        }
    }
}
