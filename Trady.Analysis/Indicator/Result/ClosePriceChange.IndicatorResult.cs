using System;
using System.Collections.Generic;
using Trady.Core;
using static Trady.Analysis.Indicator.ClosePriceChange;

namespace Trady.Analysis.Indicator
{
    public partial class ClosePriceChange : IndicatorBase<IndicatorResult>
    {
        public class IndicatorResult : TickBase
        {
            public IndicatorResult(DateTime dateTime, decimal? change) : base(dateTime)
            {
                Change = change;
            }

            public decimal? Change { get; private set; }
        }
    }
}
