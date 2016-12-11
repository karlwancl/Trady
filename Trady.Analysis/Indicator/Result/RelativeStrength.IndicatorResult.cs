using System;
using System.Collections.Generic;
using Trady.Core;

namespace Trady.Analysis.Indicator
{
    public partial class RelativeStrength : IndicatorBase
    {
        public class IndicatorResult : TickBase
        {
            public IndicatorResult(DateTime dateTime, decimal rs) : base(dateTime)
            {
                Rs = rs;
            }

            public decimal Rs { get; private set; }
        }
    }
}
