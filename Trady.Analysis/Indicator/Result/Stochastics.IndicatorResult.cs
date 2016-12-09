using System;
using System.Collections.Generic;

namespace Trady.Analysis.Indicator
{
    public partial class Stochastics
    {

        public class IndicatorResult : IndicatorResultBase
        {
            public IndicatorResult(DateTime dateTime, decimal k, decimal d, decimal j) 
                : base(dateTime, CreateValuesDictionary(k, d, j))
            {
            }

            private static IDictionary<string, decimal> CreateValuesDictionary(decimal k, decimal d, decimal j)
            {
                var values = new Dictionary<string, decimal>();
                values.Add(KTag, k);
                values.Add(DTag, d);
                values.Add(JTag, j);
                return values;
            }

            public decimal K => Values[KTag];

            public decimal D => Values[DTag];

            public decimal J => Values[JTag];
        }
    }
}
