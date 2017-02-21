using System;
using Trady.Core;
using static Trady.Analysis.Indicator.GenericExponentialMovingAverage;

namespace Trady.Analysis.Indicator
{
    public partial class GenericExponentialMovingAverage : CacheIndicatorBase<IndicatorResult>
    {
        public class IndicatorResult : TickBase
        {
            public IndicatorResult(DateTime dateTime, decimal? ema) : base(dateTime)
            {
                Ema = ema;
            }

            public decimal? Ema { get; private set; }
        }
    }
}