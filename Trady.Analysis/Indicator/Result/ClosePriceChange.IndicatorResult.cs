using System;
using Trady.Core;
using static Trady.Analysis.Indicator.ClosePriceChange;

namespace Trady.Analysis.Indicator
{
    public partial class ClosePriceChange : IndicatorBase<IndicatorResult>
    {
        public class IndicatorResult : IndicatorResultBase
        {
            public IndicatorResult(DateTime dateTime, decimal? change) : base(dateTime, change)
            {
            }

            public decimal? Change => Values[0];
        }
    }
}