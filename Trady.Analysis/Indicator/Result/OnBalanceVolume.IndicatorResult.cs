using System;
using Trady.Core;
using static Trady.Analysis.Indicator.OnBalanceVolume;

namespace Trady.Analysis.Indicator
{
    public partial class OnBalanceVolume : CacheIndicatorBase<IndicatorResult>
    {
        public class IndicatorResult : TickBase
        {
            public IndicatorResult(DateTime dateTime, long? obv) : base(dateTime)
            {
                Obv = obv;
            }

            public long? Obv { get; private set; }
        }
    }
}