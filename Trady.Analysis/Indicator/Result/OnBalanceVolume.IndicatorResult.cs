using System;
using Trady.Core;
using static Trady.Analysis.Indicator.OnBalanceVolume;

namespace Trady.Analysis.Indicator
{
    public partial class OnBalanceVolume : CacheIndicatorBase<IndicatorResult>
    {
        public class IndicatorResult : TickBase
        {
            public IndicatorResult(DateTime dateTime, decimal? obv) : base(dateTime)
            {
                Obv = obv;
            }

            public decimal? Obv { get; private set; }
        }
    }
}