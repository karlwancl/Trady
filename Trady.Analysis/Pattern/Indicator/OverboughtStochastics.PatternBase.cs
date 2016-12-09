using Trady.Analysis.Indicator;
using Trady.Core;

namespace Trady.Analysis.Pattern.Indicator
{
    public partial class OverboughtStochastics
    {
        public abstract class PatternBase : PatternBase<NonDirectionalPatternResult>
        {
            private Stochastics.IndicatorBase _stoIndicator;

            public PatternBase(Equity series, Stochastics.IndicatorBase stoIndicator) : base(series)
            {
                _stoIndicator = stoIndicator;
            }

            protected override IAnalyticResult<bool> ComputeResultByIndex(int index)
            {
                var result = _stoIndicator.ComputeByIndex(index);
                return new NonDirectionalPatternResult(Series[index].DateTime, result.K >= 80);
            }
        }
    }
}
