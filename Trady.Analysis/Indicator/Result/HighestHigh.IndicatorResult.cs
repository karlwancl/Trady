using System;
using System.Collections.Generic;
using Trady.Core;
using static Trady.Analysis.Indicator.HighestHigh;

namespace Trady.Analysis.Indicator
{
    public partial class HighestHigh : IndicatorBase<IndicatorResult>
    {
        public class IndicatorResult : TickBase
        {
            public IndicatorResult(DateTime dateTime, decimal? highestHigh) : base(dateTime) 
            {
                HighestHigh = highestHigh;
            }

            public decimal? HighestHigh { get; private set; }
        }
    }
}
