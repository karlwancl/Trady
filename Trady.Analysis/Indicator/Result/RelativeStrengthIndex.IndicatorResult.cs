using System;
using System.Collections.Generic;
using Trady.Core;

namespace Trady.Analysis.Indicator
{
    public partial class RelativeStrengthIndex : IndicatorBase
    {
        public class IndicatorResult : TickBase
        {
            public IndicatorResult(DateTime dateTime, decimal rsi) : base(dateTime)
            {
                Rsi = rsi;
            }

            public decimal Rsi { get; private set; }
        }
    }
}
