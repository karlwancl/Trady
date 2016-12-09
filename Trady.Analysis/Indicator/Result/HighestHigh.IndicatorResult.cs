using System;
using System.Collections.Generic;

namespace Trady.Analysis.Indicator
{
    public partial class HighestHigh : IndicatorBase
    {

        public class IndicatorResult : IndicatorResultBase
        {
            public IndicatorResult(DateTime dateTime, decimal highestHigh) 
                : base(dateTime, new Dictionary<string, decimal> { { HighestHighTag, highestHigh } })
            {
            }

            public decimal HighestHigh => Values[HighestHighTag];
        }
    }
}
