using System;

namespace Trady.Analysis.Indicator
{
    public partial class Stochastics
    {
        public class IndicatorResult : IndicatorResultBase
        {
            public IndicatorResult(DateTime dateTime, decimal? k, decimal? d, decimal? j) : base(dateTime, k, d, j)
            {
            }

            public decimal? K => Values[0];

            public decimal? D => Values[1];

            public decimal? J => Values[2];
        }
    }
}