using System;
using static Trady.Analysis.Indicator.OnBalanceVolume;

namespace Trady.Analysis.Indicator
{
    public partial class OnBalanceVolume : CummulativeIndicatorBase<IndicatorResult>
    {
        public class IndicatorResult : IndicatorResultBase
        {
            public IndicatorResult(DateTime dateTime, decimal? obv) : base(dateTime, obv)
            {
            }

            public decimal? Obv => Values[0];
        }
    }
}