using System;
using System.Collections.Generic;

namespace Trady.Analysis.Indicator
{
    public partial class RawStochasticsValue : IndicatorBase
    {

        public class IndicatorResult : IndicatorResultBase
        {
            public IndicatorResult(DateTime dateTime, decimal rsv) 
                : base(dateTime, new Dictionary<string, decimal> { { RsvTag, rsv } })
            {
            }

            public decimal Rsv => Values[RsvTag];
        }
    }
}
