using System;
using Trady.Core;
using static Trady.Analysis.Indicator.SimpleMovingAverageOscillator;

namespace Trady.Analysis.Indicator
{
    public partial class SimpleMovingAverageOscillator : IndicatorBase<IndicatorResult>
    {
        public class IndicatorResult : TickBase
        {
            public IndicatorResult(DateTime dateTime, decimal? osc) : base(dateTime)
            {
                Osc = osc;
            }

            public decimal? Osc { get; private set; }
        }
    }
}