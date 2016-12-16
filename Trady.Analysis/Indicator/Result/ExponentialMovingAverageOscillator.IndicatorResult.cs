using System;
using System.Collections.Generic;
using Trady.Core;
using static Trady.Analysis.Indicator.ExponentialMovingAverageOscillator;

namespace Trady.Analysis.Indicator
{
    public partial class ExponentialMovingAverageOscillator : IndicatorBase<IndicatorResult>
    {
        public class IndicatorResult : TickBase
        {
            public IndicatorResult(DateTime dateTime, decimal? osc): base(dateTime)
            {
                Osc = osc;
            }

            public decimal? Osc { get; private set; }
        }
    }
}
