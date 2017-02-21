using System;
using Trady.Core;
using static Trady.Analysis.Indicator.HistoricalHighestClose;

namespace Trady.Analysis.Indicator
{
    public partial class HistoricalHighestClose : IndicatorBase<IndicatorResult>
    {
        public class IndicatorResult : TickBase
        {
            public IndicatorResult(DateTime dateTime, decimal? historicalHighestClose) : base(dateTime)
            {
                HistoricalHighestClose = historicalHighestClose;
            }

            public decimal? HistoricalHighestClose { get; private set; }
        }
    }
}
