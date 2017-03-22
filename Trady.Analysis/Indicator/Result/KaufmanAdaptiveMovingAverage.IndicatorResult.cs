using System;
using static Trady.Analysis.Indicator.KaufmanAdaptiveMovingAverage;

namespace Trady.Analysis.Indicator
{
    public partial class KaufmanAdaptiveMovingAverage : IndicatorBase<IndicatorResult>
    {
        public class IndicatorResult : IndicatorResultBase
        {
            public IndicatorResult(DateTime dateTime, decimal? kama) : base(dateTime, kama)
            {
            }

            public decimal? Kama => Values[0];
        }
    }
}