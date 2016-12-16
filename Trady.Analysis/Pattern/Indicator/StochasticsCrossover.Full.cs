using Trady.Analysis.Indicator;
using Trady.Core;

namespace Trady.Analysis.Pattern.Indicator
{
    public partial class StochasticsCrossover
    {
        public class Full : IndicatorBase
        {
            public Full(Equity equity, int periodCount, int smaPeriodCountK, int smaPeriodCountD)
                : base(equity, new Stochastics.Full(equity, periodCount, smaPeriodCountK, smaPeriodCountD))
            {
            }
        }
    }
}
