using System;
using System.Collections.Generic;
using Trady.Core;

namespace Trady.Analysis.Indicator
{
    public partial class LowestLow : IndicatorBase
    {
        public class IndicatorResult : TickBase
        {
            public IndicatorResult(DateTime dateTime, decimal lowestLow) : base(dateTime)
            {
                LowestLow = lowestLow;
            }

            public decimal LowestLow { get; private set; }
        }
    }
}
