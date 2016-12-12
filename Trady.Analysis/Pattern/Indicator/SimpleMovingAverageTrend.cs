using System;
using System.Collections.Generic;
using System.Text;
using Trady.Analysis.Indicator;
using Trady.Analysis.Pattern.Helper;
using Trady.Core;

namespace Trady.Analysis.Pattern.Indicator
{
    public class SimpleMovingAverageTrend : AnalyticBase<MultistateResult<Trend?>>
    {
        private SimpleMovingAverage _smaIndicator;

        public SimpleMovingAverageTrend(Equity equity, int periodCount) : base(equity)
        {
            _smaIndicator = new SimpleMovingAverage(equity, periodCount);
        }

        public override MultistateResult<Trend?> ComputeByIndex(int index)
        {
            if (index < 1)
                return new MultistateResult<Trend?>(Equity[index].DateTime, null);

            var currentValue = _smaIndicator.ComputeByIndex(index).Sma;
            var previousValue = _smaIndicator.ComputeByIndex(index - 1).Sma;

            return new MultistateResult<Trend?>(Equity[index].DateTime, 
                ResultExt.IsTrending(currentValue - previousValue));
        }
    }
}
