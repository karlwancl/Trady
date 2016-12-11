using System;
using System.Collections.Generic;
using Trady.Core;

namespace Trady.Analysis.Indicator
{
    public partial class SimpleMovingAverageOscillator : IndicatorBase
    {
        public class IndicatorResult : TickBase
        {
            public IndicatorResult(DateTime dateTime, decimal osc) : base(dateTime)
            {
                Osc = osc;
            }

            public decimal Osc { get; private set; }
        }
    }
}
