using System;
using Trady.Core;
using static Trady.Analysis.Indicator.HighestClose;

namespace Trady.Analysis.Indicator
{
    public partial class HighestClose : IndicatorBase<IndicatorResult>
    {
        public class IndicatorResult : TickBase
        {
            public IndicatorResult(DateTime dateTime, decimal? highestClose) : base(dateTime)
            {
                HighestClose = highestClose;
            }

            public decimal? HighestClose { get; private set; }
        }
    }
}
