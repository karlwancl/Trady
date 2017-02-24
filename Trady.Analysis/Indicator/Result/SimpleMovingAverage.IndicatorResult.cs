using System;
using Trady.Core;
using static Trady.Analysis.Indicator.SimpleMovingAverage;

namespace Trady.Analysis.Indicator
{
    public partial class SimpleMovingAverage : IndicatorBase<IndicatorResult>
    {
        public class IndicatorResult : IndicatorResultBase
        {
            public IndicatorResult(DateTime dateTime, decimal? sma) : base(dateTime, sma)
            {
            }

            public decimal? Sma => Values[0];
        }
    }
}