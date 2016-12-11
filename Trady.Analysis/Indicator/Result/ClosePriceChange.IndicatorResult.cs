using System;
using System.Collections.Generic;
using Trady.Core;

namespace Trady.Analysis.Indicator
{
    public partial class ClosePriceChange : IndicatorBase
    {
        public class IndicatorResult : TickBase
        {
            public IndicatorResult(DateTime dateTime, decimal change) : base(dateTime)
            {
                Change = change;
            }

            public decimal Change { get; private set; }
        }
    }
}
