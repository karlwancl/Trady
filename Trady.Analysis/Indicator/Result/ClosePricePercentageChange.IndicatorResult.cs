using System;
using Trady.Core;
using static Trady.Analysis.Indicator.ClosePricePercentageChange;

namespace Trady.Analysis.Indicator
{
    public partial class ClosePricePercentageChange : IndicatorBase<IndicatorResult>
    {
        public class IndicatorResult : IndicatorResultBase
        {
            public IndicatorResult(DateTime dateTime, decimal? percentageChange) : base(dateTime, percentageChange)
            {
            }

            public decimal? PercentageChange => Values[0];
        }
    }
}