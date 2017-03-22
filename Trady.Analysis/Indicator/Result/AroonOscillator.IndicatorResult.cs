using System;
using static Trady.Analysis.Indicator.AroonOscillator;

namespace Trady.Analysis.Indicator
{
    public partial class AroonOscillator : IndicatorBase<IndicatorResult>
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