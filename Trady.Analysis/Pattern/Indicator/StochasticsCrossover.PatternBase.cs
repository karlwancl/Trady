using Trady.Analysis.Indicator;
using Trady.Core;

namespace Trady.Analysis.Pattern.Indicator
{
    public partial class StochasticsCrossover
    {
        public abstract class PatternBase : PatternBase<IsMatchedMultistateResult<Trend>>
        {
            private Stochastics.IndicatorBase _stoIndicator;

            protected PatternBase(Equity equity, Stochastics.IndicatorBase stoIndicator) : base(equity)
            {
                _stoIndicator = stoIndicator;
            }

            protected override IAnalyticResult<bool> ComputeResultByIndex(int index)
            {
                if (index < 1)
                    return new IsMatchedMultistateResult<Trend>(Equity[index].DateTime, false, Trend.NonTrended);

                var latest = _stoIndicator.ComputeByIndex(index);
                var secondLatest = _stoIndicator.ComputeByIndex(index - 1);

                var latestKdOsc = latest.K - latest.D;
                var secondLatestKsOsc = secondLatest.K - secondLatest.D;

                return new IsMatchedMultistateResult<Trend>(Equity[index].DateTime, latestKdOsc * secondLatestKsOsc < 0, GetTrend(latestKdOsc));
            }

            protected Trend GetTrend(decimal value)
            {
                if (value > 0) return Trend.Bullish;
                if (value < 0) return Trend.Bearish;
                return Trend.NonTrended;
            }
        }
    }
}
