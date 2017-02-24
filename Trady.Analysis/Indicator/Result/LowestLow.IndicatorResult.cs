using System;
using Trady.Core;
using static Trady.Analysis.Indicator.LowestLow;

namespace Trady.Analysis.Indicator
{
    public partial class LowestLow : IndicatorBase<IndicatorResult>
    {
        public class IndicatorResult : IndicatorResultBase
        {
            public IndicatorResult(DateTime dateTime, decimal? lowestLow) : base(dateTime, lowestLow)
            {
            }

            public decimal? LowestLow => Values[0];
        }
    }
}