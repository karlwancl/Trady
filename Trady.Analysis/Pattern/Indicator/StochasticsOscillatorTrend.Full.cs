using System;
using System.Collections.Generic;
using System.Text;
using Trady.Core;
using Trady.Analysis.Indicator;

namespace Trady.Analysis.Pattern.Indicator
{
    public partial class StochasticsOscillatorTrend
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
