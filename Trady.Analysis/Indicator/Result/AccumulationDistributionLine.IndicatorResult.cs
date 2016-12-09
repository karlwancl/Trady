using System;
using System.Collections.Generic;

namespace Trady.Analysis.Indicator
{
    public partial class AccumulationDistributionLine : CachedIndicatorBase
    {
        public class IndicatorResult : IndicatorResultBase
        {
            public IndicatorResult(DateTime dateTime, decimal accumDist)
                : base(dateTime, new Dictionary<string, decimal> { { AccumDistTag, accumDist } })
            {
            }

            public decimal AccumDist => Values[AccumDistTag];
        }
    }
}
