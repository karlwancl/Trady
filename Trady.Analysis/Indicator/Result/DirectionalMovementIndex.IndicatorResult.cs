using System;
using Trady.Core;
using static Trady.Analysis.Indicator.DirectionalMovementIndex;

namespace Trady.Analysis.Indicator
{
    public partial class DirectionalMovementIndex : IndicatorBase<IndicatorResult>
    {
        public class IndicatorResult : IndicatorResultBase
        {
            public IndicatorResult(DateTime dateTime, decimal? pdi, decimal? mdi, decimal? adx, decimal? adxr) :
                base(dateTime, pdi, mdi, adx, adxr)
            {
            }

            public decimal? Pdi => Values[0];

            public decimal? Mdi => Values[1];

            public decimal? Adx => Values[2];

            public decimal? Adxr => Values[3];
        }
    }
}