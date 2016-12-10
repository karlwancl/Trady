using System;
using System.Collections.Generic;

namespace Trady.Analysis.Indicator
{
    public partial class AverageTrueRange : IndicatorBase
    {

        public class IndicatorResult : IndicatorResultBase
        {
            public IndicatorResult(DateTime dateTime, decimal atr)
                : base(dateTime, new Dictionary<string, decimal> { { AtrTag, atr } })
            {
            }

            public decimal Atr => Values[AtrTag];
        }
    }
}
