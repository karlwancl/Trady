using System;
using System.Collections.Generic;
using System.Text;
using Trady.Analysis.Indicator;
using Trady.Core;

namespace Trady.Analysis.Pattern.Indicator
{
    public class IsAboveExponentialMovingAverage : PatternBase<IsMatchedResult>
    {
        private ExponentialMovingAverage _emaIndicator;

        public IsAboveExponentialMovingAverage(Equity equity, int periodCount) : base(equity)
        {
            _emaIndicator = new ExponentialMovingAverage(equity, periodCount);
        }

        protected override TickBase ComputeResultByIndex(int index)
        {
            var result = _emaIndicator.ComputeByIndex(index);
            return new IsMatchedResult(Equity[index].DateTime, Equity[index].Close >= result.Ema);
        }
    }
}
