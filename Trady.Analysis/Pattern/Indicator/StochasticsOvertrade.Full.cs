using Trady.Analysis.Indicator;
using Trady.Core;

namespace Trady.Analysis.Pattern.Indicator
{
    public partial class StochasticsOvertrade
    {

        public class Full : PatternBase
        {
            public Full(Equity equity, int periodCount, int smaPeriodCountK, int smaPeriodCountD)
                : base(equity, new Stochastics.Full(equity, periodCount, smaPeriodCountK, smaPeriodCountD))
            {
            }
        }
    }
}
