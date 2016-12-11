using System;
using System.Collections.Generic;
using Trady.Core;

namespace Trady.Analysis.Indicator
{
    public partial class MovingAverageConvergenceDivergence : IndicatorBase
    {
        public class IndicatorResult : TickBase
        {
            public IndicatorResult(DateTime dateTime, decimal dif, decimal dem, decimal osc) : base(dateTime)
            {
                Dif = dif;
                Dem = dem;
                Osc = osc;
            }

            public decimal Dif { get; private set; }

            public decimal Dem { get; private set; }

            public decimal Osc { get; private set; }
        }
    }
}
