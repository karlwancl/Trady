using System;
using System.Collections.Generic;

namespace Trady.Analysis.Indicator
{
    public partial class SimpleMovingAverage : IndicatorBase
    {

        public class IndicatorResult : IndicatorResultBase
        {
            public IndicatorResult(DateTime dateTime, decimal sma)
                : base(dateTime, new Dictionary<string, decimal> { { SmaTag, sma } })
            {
            }

            public decimal Sma => Values[SmaTag];
        }
    }
}
