using System;
using static Trady.Analysis.Indicator.HighestHigh;

namespace Trady.Analysis.Indicator
{
    public partial class HighestHigh : IndicatorBase<IndicatorResult>
    {
        public class IndicatorResult : IndicatorResultBase
        {
            public IndicatorResult(DateTime dateTime, decimal? highestHigh) : base(dateTime, highestHigh)
            {
            }

            public decimal? HighestHigh => Values[0];
        }
    }
}