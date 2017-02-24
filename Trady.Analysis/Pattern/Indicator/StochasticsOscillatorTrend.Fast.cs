using System;
using System.Collections.Generic;
using System.Text;
using Trady.Core;
using Trady.Analysis.Indicator;

namespace Trady.Analysis.Pattern.Indicator
{
    public partial class StochasticsOscillatorTrend
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
