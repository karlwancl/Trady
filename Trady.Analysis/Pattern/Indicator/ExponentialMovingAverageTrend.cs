using System;
using System.Collections.Generic;
using System.Text;
using Trady.Analysis.Indicator;
using Trady.Analysis.Pattern.Helper;
using Trady.Core;

namespace Trady.Analysis.Pattern.Indicator
{
    public class ExponentialMovingAverageTrend : AnalyticBase<MultistateResult<Trend?>>
    {
        private ExponentialMovingAverage _emaIndicator;

        public ExponentialMovingAverageTrend(Equity equity, int periodCount) : base(equity)
        {
            _emaIndicator = new ExponentialMovingAverage(equity, periodCount);
        }

        public override MultistateResult<Trend?> ComputeByIndex(int index)
        {
            var result = _emaIndicator.ComputeByIndex(index);
            return new MultistateResult<Trend?>(Equity[index].DateTime, ResultExt.IsTrending(result.Ema));
        }
    }
}
