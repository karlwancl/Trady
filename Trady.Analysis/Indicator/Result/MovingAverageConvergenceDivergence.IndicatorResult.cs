using System;
using Trady.Core;
using static Trady.Analysis.Indicator.MovingAverageConvergenceDivergence;

namespace Trady.Analysis.Indicator
{
    public partial class MovingAverageConvergenceDivergence : IndicatorBase<IndicatorResult>
    {
        public class IndicatorResult : IndicatorResultBase
        {
            public IndicatorResult(DateTime dateTime, decimal? dif, decimal? dem, decimal? osc) : base(dateTime, dif, dem, osc)
            {
            }

            public decimal? Dif => Values[0];

            public decimal? Dem => Values[1];

            public decimal? Osc => Values[2];
        }
    }
}