using System;
using System.Collections.Generic;
using Trady.Core;

namespace Trady.Analysis.Indicator
{
    public partial class HighestHigh : IndicatorBase
    {
        public class IndicatorResult : TickBase
        {
            public IndicatorResult(DateTime dateTime, decimal highestHigh) : base(dateTime) 
            {
                HighestHigh = highestHigh;
            }

            public decimal HighestHigh { get; private set; }
        }
    }
}
