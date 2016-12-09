using System;
using System.Collections.Generic;

namespace Trady.Analysis.Indicator
{
    public partial class ClosePriceChange : IndicatorBase
    {

        public class IndicatorResult : IndicatorResultBase
        {
            public IndicatorResult(DateTime dateTime, decimal change)
                : base(dateTime, new Dictionary<string, decimal> { { ChangeTag, change } })
            {
            }

            public decimal Change => Values[ChangeTag];
        }
    }
}
