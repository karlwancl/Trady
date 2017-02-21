using System;
using Trady.Core;
using static Trady.Analysis.Indicator.ChandelierExit;

namespace Trady.Analysis.Indicator
{
    public partial class ChandelierExit : IndicatorBase<IndicatorResult>
    {
        public class IndicatorResult : TickBase
        {
            public IndicatorResult(DateTime dateTime, decimal? @long, decimal? @short) : base(dateTime)
            {
                Long = @long;
                Short = @short;
            }

            public decimal? Long { get; private set; }

            public decimal? Short { get; private set; }
        }
    }
}