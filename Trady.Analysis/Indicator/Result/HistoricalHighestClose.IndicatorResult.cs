using System;
using Trady.Core;
using static Trady.Analysis.Indicator.HistoricalHighestClose;

namespace Trady.Analysis.Indicator
{
    public partial class HistoricalHighestClose : IndicatorBase<IndicatorResult>
    {
        public class IndicatorResult : IndicatorResultBase
        {
            public IndicatorResult(DateTime dateTime, decimal? historicalHighestClose) : base(dateTime, historicalHighestClose)
            {
            }

            public decimal? HistoricalHighestClose => Values[0];
        }
    }
}
