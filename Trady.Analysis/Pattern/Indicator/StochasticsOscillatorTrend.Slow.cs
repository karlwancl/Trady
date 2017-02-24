using System;
using System.Collections.Generic;
using System.Text;
using Trady.Core;
using Trady.Analysis.Indicator;

namespace Trady.Analysis.Pattern.Indicator
{
    public partial class StochasticsOscillatorTrend
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
