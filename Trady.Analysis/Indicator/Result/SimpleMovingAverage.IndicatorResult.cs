using System;
using System.Collections.Generic;
using Trady.Core;

namespace Trady.Analysis.Indicator
{
    public partial class SimpleMovingAverage : IndicatorBase
    {
        public class IndicatorResult : TickBase
        {
            public IndicatorResult(DateTime dateTime, decimal sma) : base(dateTime)
            {
                Sma = sma;
            }

            public decimal Sma { get; private set; }
        }
    }
}
