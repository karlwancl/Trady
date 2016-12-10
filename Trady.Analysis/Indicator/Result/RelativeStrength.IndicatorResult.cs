using System;
using System.Collections.Generic;

namespace Trady.Analysis.Indicator
{
    public partial class RelativeStrength : IndicatorBase
    {
        public class IndicatorResult : IndicatorResultBase
        {
            public IndicatorResult(DateTime dateTime, decimal rs)
                : base(dateTime, new Dictionary<string, decimal> { { RsTag, rs } })
            {
            }

            public decimal Rs => Values[RsTag];
        }
    }
}
