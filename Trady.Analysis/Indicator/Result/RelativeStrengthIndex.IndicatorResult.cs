using System;
using System.Collections.Generic;
using Trady.Core;
using static Trady.Analysis.Indicator.RelativeStrengthIndex;

namespace Trady.Analysis.Indicator
{
    public partial class RelativeStrengthIndex : IndicatorBase<IndicatorResult>
    {
        public class IndicatorResult : TickBase
        {
            public IndicatorResult(DateTime dateTime, decimal? rsi) : base(dateTime)
            {
                Rsi = rsi;
            }

            public decimal? Rsi { get; private set; }
        }
    }
}
