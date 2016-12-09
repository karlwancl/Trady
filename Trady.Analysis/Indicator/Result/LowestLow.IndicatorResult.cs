using System;
using System.Collections.Generic;

namespace Trady.Analysis.Indicator
{
    public partial class LowestLow : IndicatorBase
    {

        public class IndicatorResult : IndicatorResultBase
        {
            public IndicatorResult(DateTime dateTime, decimal lowestLow)
                : base(dateTime, new Dictionary<string, decimal> { { LowestLowTag, lowestLow } })
            {
            }

            public decimal LowestLow => Values[LowestLowTag];
        }
    }
}
