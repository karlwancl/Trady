using Trady.Analysis.Indicator;
using Trady.Core;

namespace Trady.Analysis.Pattern.Indicator
{
    public partial class OverboughtStochastics
    {

        public class Full : PatternBase
        {
            public Full(Equity series, int periodCount, int smaPeriodCountK, int smaPeriodCountD)
                : base(series, new Stochastics.Full(series, periodCount, smaPeriodCountK, smaPeriodCountD))
            {
            }
        }
    }
}
