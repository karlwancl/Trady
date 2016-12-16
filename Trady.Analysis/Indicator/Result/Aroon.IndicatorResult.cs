using System;
using Trady.Core;
using static Trady.Analysis.Indicator.Aroon;

namespace Trady.Analysis.Indicator
{
    public partial class Aroon : IndicatorBase<IndicatorResult>
    {
        public class IndicatorResult : TickBase
        {
            public IndicatorResult(DateTime dateTime, decimal? up, decimal? down, decimal? osc) : base(dateTime)
            {
                Up = up;
                Down = down;
                Osc = osc;
            }

            public decimal? Up { get; private set; }

            public decimal? Down { get; private set; }

            public decimal? Osc { get; private set; }
        }
    }
}
