using System;
using static Trady.Analysis.Indicator.HistoricalHighestHigh;

namespace Trady.Analysis.Indicator
{
    public partial class HistoricalHighestHigh : IndicatorBase<IndicatorResult>
    {
        public class IndicatorResult : IndicatorResultBase
        {
            public IndicatorResult(DateTime dateTime, decimal? historicalHighestHigh) : base(dateTime, historicalHighestHigh)
            {
            }

            public decimal? HistoricalHighestHigh => Values[0];
        }
    }
}