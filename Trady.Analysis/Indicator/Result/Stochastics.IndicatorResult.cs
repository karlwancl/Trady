using System;
using System.Collections.Generic;
using Trady.Core;

namespace Trady.Analysis.Indicator
{
    public partial class Stochastics
    {
        public class IndicatorResult : TickBase
        {
            public IndicatorResult(DateTime dateTime, decimal? k, decimal? d, decimal? j) : base(dateTime)
            {
                K = k;
                D = d;
                J = j;
            }

            public decimal? K { get; private set; }

            public decimal? D { get; private set; }

            public decimal? J { get; private set; }
        }
    }
}
