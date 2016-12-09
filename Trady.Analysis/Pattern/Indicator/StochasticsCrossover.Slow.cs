using Trady.Analysis.Indicator;
using Trady.Core;

namespace Trady.Analysis.Pattern.Indicator
{
    public partial class StochasticsCrossover
    {
        public class Slow : PatternBase
        {
            public Slow(Equity series, int periodCount, int smaPeriodCountD)
                : base(series, new Stochastics.Slow(series, periodCount, smaPeriodCountD))
            {
            }
        }
    }
}
