using System;
using System.Collections.Generic;

namespace Trady.Analysis.Indicator
{
    public partial class OnBalanceVolume : CachedIndicatorBase
    {

        public class IndicatorResult : IndicatorResultBase
        {
            public IndicatorResult(DateTime dateTime, decimal obv) 
                : base(dateTime, new Dictionary<string, decimal> { { ObvTag, obv } })
            {
            }

            public decimal Obv => Values[ObvTag];
        }
    }
}
