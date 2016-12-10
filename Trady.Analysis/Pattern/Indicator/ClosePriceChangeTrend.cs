using System;
using System.Collections.Generic;
using System.Text;
using Trady.Analysis.Indicator;
using Trady.Core;

namespace Trady.Analysis.Pattern.Indicator
{
    public class ClosePriceChangeTrend : PatternBase<MultistateResult<Trend>>
    {
        private ClosePriceChange _closePriceChangeIndicator;

        public ClosePriceChangeTrend(Equity equity) : base(equity)
        {
            _closePriceChangeIndicator = new ClosePriceChange(equity);
        }

        protected override IAnalyticResult<bool> ComputeResultByIndex(int index)
        {
            var latest = _closePriceChangeIndicator.ComputeByIndex(index);
            return new MultistateResult<Trend>(Equity[index].DateTime, GetTrend(latest.Change));
        }

        private Trend GetTrend(decimal change)
        {
            if (change > 0) return Trend.Bullish;
            if (change < 0) return Trend.Bearish;
            return Trend.NonTrended;
        }
    }
}
