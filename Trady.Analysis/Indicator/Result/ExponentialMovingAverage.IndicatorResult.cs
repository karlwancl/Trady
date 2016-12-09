using System;
using System.Collections.Generic;

namespace Trady.Analysis.Indicator
{
    public partial class ExponentialMovingAverage : CachedIndicatorBase
    {

        public class IndicatorResult : IndicatorResultBase
        {
            public IndicatorResult(DateTime dateTime, decimal ema) 
                : base(dateTime, new Dictionary<string, decimal> { { EmaTag, ema } })
            {
            }

            public decimal Ema => Values[EmaTag];
        }
    }
}
