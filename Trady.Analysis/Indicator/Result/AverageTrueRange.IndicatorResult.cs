using System;
using static Trady.Analysis.Indicator.AverageTrueRange;

namespace Trady.Analysis.Indicator
{
    public partial class AverageTrueRange : IndicatorBase<IndicatorResult>
    {
        public class IndicatorResult : IndicatorResultBase
        {
            public IndicatorResult(DateTime dateTime, decimal? atr) : base(dateTime, atr)
            {
            }

            public decimal? Atr => Values[0];
        }
    }
}