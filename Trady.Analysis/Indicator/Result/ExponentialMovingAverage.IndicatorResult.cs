using System;
using System.Collections.Generic;
using Trady.Core;
using static Trady.Analysis.Indicator.ExponentialMovingAverage;

namespace Trady.Analysis.Indicator
{
    public partial class ExponentialMovingAverage : IndicatorBase<IndicatorResult>
    {
        public class IndicatorResult : TickBase
        {
            public IndicatorResult(DateTime dateTime, decimal? ema)  : base(dateTime)
            {
                Ema = ema;
            }

            public decimal? Ema { get; private set; }
        }
    }
}
