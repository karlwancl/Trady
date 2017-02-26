using System;
using static Trady.Analysis.Indicator.SimpleMovingAverageOscillator;

namespace Trady.Analysis.Indicator
{
    public partial class SimpleMovingAverageOscillator : IndicatorBase<IndicatorResult>
    {
        public class IndicatorResult : IndicatorResultBase
        {
            public IndicatorResult(DateTime dateTime, decimal? osc) : base(dateTime, osc)
            {
            }

            public decimal? Osc => Values[0];
        }
    }
}