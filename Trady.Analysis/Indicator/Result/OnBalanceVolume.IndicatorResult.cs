using System;
using System.Collections.Generic;
using Trady.Core;

namespace Trady.Analysis.Indicator
{
    public partial class OnBalanceVolume : CachedIndicatorBase
    {
        public class IndicatorResult : TickBase
        {
            public IndicatorResult(DateTime dateTime, decimal obv) : base(dateTime)
            {
                Obv = obv;
            }

            public decimal Obv { get; private set; }
        }
    }
}
