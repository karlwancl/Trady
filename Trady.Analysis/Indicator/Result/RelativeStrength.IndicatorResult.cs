using System;
using System.Collections.Generic;

namespace Trady.Analysis.Indicator
{
    public partial class RelativeStrength : CachedIndicatorBase
    {

        public class IndicatorResult : IndicatorResultBase
        {
            public IndicatorResult(DateTime dateTime, decimal gain, decimal loss, decimal rs)
                : base(dateTime, CreateValuesDictionary(gain, loss, rs))
            {
            }

            private static IDictionary<string, decimal> CreateValuesDictionary(decimal gain, decimal loss, decimal rs)
            {
                var values = new Dictionary<string, decimal>();
                values.Add(GainTag, gain);
                values.Add(LossTag, loss);
                values.Add(RsTag, rs);
                return values;
            }

            public decimal Gain => Values[GainTag];

            public decimal Loss => Values[LossTag];

            public decimal Rs => Values[RsTag];
        }
    }
}
