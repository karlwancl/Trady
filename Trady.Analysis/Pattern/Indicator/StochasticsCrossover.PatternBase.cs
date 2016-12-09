using Trady.Analysis.Indicator;
using Trady.Core;

namespace Trady.Analysis.Pattern.Indicator
{
    public partial class StochasticsCrossover
    {
        public abstract class PatternBase : PatternBase<DirectionalPatternResult>
        {
            private Stochastics.IndicatorBase _stoIndicator;

            protected PatternBase(Equity series, Stochastics.IndicatorBase stoIndicator) : base(series)
            {
                _stoIndicator = stoIndicator;
            }

            protected override IAnalyticResult<bool> ComputeResultByIndex(int index)
            {
                if (index < 1)
                    return new DirectionalPatternResult(Series[index].DateTime, false, false, false);

                var latest = _stoIndicator.ComputeByIndex(index);
                var secondLatest = _stoIndicator.ComputeByIndex(index - 1);

                var latestKdOsc = latest.K - latest.D;
                var secondLatestKsOsc = secondLatest.K - secondLatest.D;

                return new DirectionalPatternResult(Series[index].DateTime, latestKdOsc * secondLatestKsOsc < 0, latestKdOsc > 0, latestKdOsc < 0);
            }
        }
    }
}
