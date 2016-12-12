using System;
using System.Collections.Generic;
using Trady.Core;
using static Trady.Analysis.Indicator.LowestLow;

namespace Trady.Analysis.Indicator
{
    public partial class LowestLow : IndicatorBase<IndicatorResult>
    {
        public class IndicatorResult : TickBase
        {
            public IndicatorResult(DateTime dateTime, decimal? lowestLow) : base(dateTime)
            {
                LowestLow = lowestLow;
            }

            public decimal? LowestLow { get; private set; }
        }
    }
}
