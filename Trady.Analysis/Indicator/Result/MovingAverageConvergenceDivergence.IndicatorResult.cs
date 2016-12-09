using System;
using System.Collections.Generic;

namespace Trady.Analysis.Indicator
{
    public partial class MovingAverageConvergenceDivergence : CachedIndicatorBase
    {

        public class IndicatorResult : IndicatorResultBase
        {
            public IndicatorResult(DateTime dateTime, decimal dif, decimal dem, decimal osc) 
                : base(dateTime, CreateValuesDictionary(dif, dem, osc))
            {
            }

            private static IDictionary<string, decimal> CreateValuesDictionary(decimal dif, decimal dem, decimal osc)
            {
                var values = new Dictionary<string, decimal>();
                values.Add(DifTag, dif);
                values.Add(DemTag, dem);
                values.Add(OscTag, osc);
                return values;
            }

            public decimal Dif => Values[DifTag];

            public decimal Dem => Values[DemTag];

            public decimal Osc => Values[OscTag];
        }
    }
}
