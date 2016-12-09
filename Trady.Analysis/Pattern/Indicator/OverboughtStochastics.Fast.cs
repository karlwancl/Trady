using Trady.Analysis.Indicator;
using Trady.Core;

namespace Trady.Analysis.Pattern.Indicator
{
    public partial class OverboughtStochastics
    {
        public class Fast : PatternBase
        {
            public Fast(Equity series, int periodCount, int smaPeriodCount) 
                : base(series, new Stochastics.Fast(series, periodCount, smaPeriodCount))
            {
            }
        }
    }
}
