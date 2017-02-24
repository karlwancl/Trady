using System;
using Trady.Core;
using static Trady.Analysis.Indicator.GenericExponentialMovingAverage;

namespace Trady.Analysis.Indicator
{
    public partial class GenericExponentialMovingAverage : CummulativeIndicatorBase<IndicatorResult>
    {
        public class IndicatorResult : IndicatorResultBase
        {
            public IndicatorResult(DateTime dateTime, decimal? ema) : base(dateTime, ema)
            {
            }

            public decimal? Ema => Values[0];
        }
    }
}