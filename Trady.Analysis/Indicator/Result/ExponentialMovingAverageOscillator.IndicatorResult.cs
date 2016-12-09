using System;
using System.Collections.Generic;

namespace Trady.Analysis.Indicator
{
    public partial class ExponentialMovingAverageOscillator : IndicatorBase
    {

        public class IndicatorResult : IndicatorResultBase
        {
            public IndicatorResult(DateTime dateTime, decimal osc)
                : base(dateTime, new Dictionary<string, decimal> { { OscTag, osc } })
            {
            }

            public decimal Osc => Values[OscTag];
        }
    }
}
