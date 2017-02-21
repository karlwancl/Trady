using System;
using Trady.Core;
using static Trady.Analysis.Indicator.ClosePricePercentageChange;

namespace Trady.Analysis.Indicator
{
    public partial class ClosePricePercentageChange : IndicatorBase<IndicatorResult>
    {
        public class IndicatorResult : TickBase
        {
            public IndicatorResult(DateTime dateTime, decimal? percentageChange) : base(dateTime)
            {
                PercentageChange = percentageChange;
            }

            public decimal? PercentageChange { get; private set; }
        }
    }
}