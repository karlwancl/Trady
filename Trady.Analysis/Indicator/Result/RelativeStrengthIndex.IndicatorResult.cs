using System;
using System.Collections.Generic;

namespace Trady.Analysis.Indicator
{
    public partial class RelativeStrengthIndex : IndicatorBase
    {

        public class IndicatorResult : IndicatorResultBase
        {
            public IndicatorResult(DateTime dateTime, decimal rsi) 
                : base(dateTime, new Dictionary<string, decimal> { { RsiTag, rsi } })
            {
            }

            public decimal Rsi => Values[RsiTag];
        }
    }
}
