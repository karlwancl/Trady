using System;
using System.Collections.Generic;
using System.Text;
using Trady.Analysis.Indicator;
using Trady.Core;

namespace Trady.Analysis.Pattern.Indicator
{
    public class IsAboveSimpleMovingAverage : PatternBase<IsMatchedResult>
    {
        private SimpleMovingAverage _smaIndicator;

        public IsAboveSimpleMovingAverage(Equity equity, int periodCount) : base(equity)
        {
            _smaIndicator = new SimpleMovingAverage(equity, periodCount);
        }

        protected override IAnalyticResult<bool> ComputeResultByIndex(int index)
        {
            var result = _smaIndicator.ComputeByIndex(index);
            return new IsMatchedResult(Equity[index].DateTime, Equity[index].Close >= result.Sma);
        }
    }
}
