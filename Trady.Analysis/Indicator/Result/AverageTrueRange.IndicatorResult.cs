using System;
using Trady.Core;
using static Trady.Analysis.Indicator.AverageTrueRange;

namespace Trady.Analysis.Indicator
{
    public partial class AverageTrueRange : IndicatorBase<IndicatorResult>
    {
        public class IndicatorResult : TickBase
        {
            public IndicatorResult(DateTime dateTime, decimal? atr) : base(dateTime)
            {
                Atr = atr;
            }

            public decimal? Atr { get; private set; }
        }
    }
}