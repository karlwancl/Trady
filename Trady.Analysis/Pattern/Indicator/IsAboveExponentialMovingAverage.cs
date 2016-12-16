using System;
using System.Collections.Generic;
using System.Text;
using Trady.Analysis.Indicator;
using Trady.Analysis.Pattern.Helper;
using Trady.Core;

namespace Trady.Analysis.Pattern.Indicator
{
    public class IsAboveExponentialMovingAverage : IndicatorBase<IsMatchedResult>
    {
        private ExponentialMovingAverage _emaIndicator;

        public IsAboveExponentialMovingAverage(Equity equity, int periodCount) 
            : base(equity, periodCount)
        {
            _emaIndicator = new ExponentialMovingAverage(equity, periodCount);
        }

        public override IsMatchedResult ComputeByIndex(int index)
        {
            var result = _emaIndicator.ComputeByIndex(index);
            return new IsMatchedResult(Equity[index].DateTime, Equity[index].Close.IsLargerThan(result.Ema));
        }
    }
}
