using System;
using System.Collections.Generic;
using Trady.Core;

namespace Trady.Analysis.Indicator
{
    public partial class AverageTrueRange : IndicatorBase
    {
        public class IndicatorResult : TickBase
        {
            public IndicatorResult(DateTime dateTime, decimal atr) : base(dateTime)
            {
                Atr = atr;
            }

            public decimal Atr { get; private set; }
        }
    }
}
