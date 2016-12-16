using System;
using System.Collections.Generic;
using Trady.Core;
using static Trady.Analysis.Indicator.RelativeStrength;

namespace Trady.Analysis.Indicator
{
    public partial class RelativeStrength : IndicatorBase<IndicatorResult>
    {
        public class IndicatorResult : TickBase
        {
            public IndicatorResult(DateTime dateTime, decimal? rs) : base(dateTime)
            {
                Rs = rs;
            }

            public decimal? Rs { get; private set; }
        }
    }
}
