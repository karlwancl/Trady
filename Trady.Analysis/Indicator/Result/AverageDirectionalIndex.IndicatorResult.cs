using System;
using System.Collections.Generic;

namespace Trady.Analysis.Indicator
{
    public partial class AverageDirectionalIndex : IndicatorBase
    {

        public class IndicatorResult : IndicatorResultBase
        {
            public IndicatorResult(DateTime dateTime, decimal adx)
                : base(dateTime, new Dictionary<string, decimal> { { AdxTag, adx } })
            {
            }

            public decimal Adx => Values[AdxTag];
        }
    }
}
