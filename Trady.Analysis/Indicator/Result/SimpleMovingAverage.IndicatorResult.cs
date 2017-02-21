using System;
using Trady.Core;
using static Trady.Analysis.Indicator.SimpleMovingAverage;

namespace Trady.Analysis.Indicator
{
    public partial class SimpleMovingAverage : IndicatorBase<IndicatorResult>
    {
        public class IndicatorResult : TickBase
        {
            public IndicatorResult(DateTime dateTime, decimal? sma) : base(dateTime)
            {
                Sma = sma;
            }

            public decimal? Sma { get; private set; }
        }
    }
}