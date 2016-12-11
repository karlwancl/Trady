using System;
using System.Collections.Generic;
using Trady.Core;

namespace Trady.Analysis.Indicator
{
    public partial class ClosePricePercentageChange : IndicatorBase
    {
        public class IndicatorResult : TickBase
        {
            public IndicatorResult(DateTime dateTime, decimal percentageChange) : base(dateTime)
            {
                PercentageChange = percentageChange;
            }

            public decimal PercentageChange { get; private set; }
        }
    }
}
