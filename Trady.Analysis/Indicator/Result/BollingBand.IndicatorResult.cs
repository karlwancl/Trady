using System;
using System.Collections.Generic;

namespace Trady.Analysis.Indicator
{
    public partial class BollingerBands : IndicatorBase
    {

        public class IndicatorResult : IndicatorResultBase
        {
            public IndicatorResult(DateTime dateTime, decimal lowerBand, decimal middleBand, decimal upperBand, decimal bandWidth) 
                : base(dateTime, ComposeDictionary(lowerBand, middleBand, upperBand, bandWidth))
            {
            }

            private static IDictionary<string, decimal> ComposeDictionary(decimal lowerBand, decimal middleBand, decimal upperBand, decimal bandWith)
            {
                var values = new Dictionary<string, decimal>();
                values.Add(LowerTag, lowerBand);
                values.Add(MiddleTag, middleBand);
                values.Add(UpperTag, upperBand);
                values.Add(WidthTag, bandWith);
                return values;
            }

            public decimal Lower => Values[LowerTag];

            public decimal Middle => Values[MiddleTag];

            public decimal Upper => Values[UpperTag];

            public decimal Width => Values[WidthTag];
        }
    }
}
