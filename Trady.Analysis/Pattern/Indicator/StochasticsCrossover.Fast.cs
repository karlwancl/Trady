using Trady.Analysis.Indicator;
using Trady.Core;

namespace Trady.Analysis.Pattern.Indicator
{
    public partial class StochasticsCrossover
    {
        public class Fast : IndicatorBase
        {
            public Fast(Equity equity, int periodCount, int smaPeriodCount)
                : base(equity, new Stochastics.Fast(equity, periodCount, smaPeriodCount))
            {
            }
        }
    }
}
