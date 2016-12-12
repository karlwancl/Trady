using System;
using System.Collections.Generic;
using Trady.Core;
using static Trady.Analysis.Indicator.DirectionalMovementIndex;

namespace Trady.Analysis.Indicator
{
    public partial class DirectionalMovementIndex : IndicatorBase<IndicatorResult>
    {
        public class IndicatorResult : TickBase
        {
            public IndicatorResult(DateTime dateTime, decimal? pdi, decimal? mdi, decimal? adx, decimal? adxr) : base(dateTime)
            {
                Pdi = pdi;
                Mdi = mdi;
                Adx = adx;
                Adxr = adxr;
            }

            public decimal? Pdi { get; private set; }

            public decimal? Mdi { get; private set; }

            public decimal? Adx { get; private set; }

            public decimal? Adxr { get; private set; }
        }
    }
}
