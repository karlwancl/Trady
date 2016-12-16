using Trady.Analysis.Indicator;
using Trady.Core;

namespace Trady.Analysis.Pattern.Indicator
{
    public partial class StochasticsOvertrade
    {
        public class Slow : IndicatorBase
        {
            public Slow(Equity equity, int periodCount, int smaPeriodCountD)
                : base(equity, new Stochastics.Slow(equity, periodCount, smaPeriodCountD))
            {
            }
        }
    }
}
