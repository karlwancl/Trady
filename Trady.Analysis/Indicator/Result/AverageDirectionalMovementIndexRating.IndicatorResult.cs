using System;
using static Trady.Analysis.Indicator.AverageDirectionalMovementIndexRating;

namespace Trady.Analysis.Indicator
{
    public partial class AverageDirectionalMovementIndexRating : IndicatorBase<IndicatorResult>
    {
        public class IndicatorResult : IndicatorResultBase
        {
            public IndicatorResult(DateTime dateTime, decimal? adxr) : base(dateTime, adxr)
            {
            }

            public decimal? Adxr => Values[0];
        }
    }
}