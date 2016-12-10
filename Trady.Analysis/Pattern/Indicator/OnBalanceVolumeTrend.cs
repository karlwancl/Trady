using System;
using System.Collections.Generic;
using System.Text;
using Trady.Analysis.Indicator;
using Trady.Core;

namespace Trady.Analysis.Pattern.Indicator
{
    public class OnBalanceVolumeTrend : PatternBase<MultistateResult<Trend>>
    {
        private OnBalanceVolume _obvIndicator;

        public OnBalanceVolumeTrend(Equity equity) : base(equity)
        {
            _obvIndicator = new OnBalanceVolume(equity);
        }

        protected override IAnalyticResult<bool> ComputeResultByIndex(int index)
        {
            if (index < 1)
                return new IsMatchedResult(Equity[index].DateTime, false);

            var latest = _obvIndicator.ComputeByIndex(index);
            var secondLatest = _obvIndicator.ComputeByIndex(index - 1);
            return new MultistateResult<Trend>(Equity[index].DateTime, GetTrend(latest.Obv, secondLatest.Obv));
        }

        private Trend GetTrend(decimal value1, decimal value2)
        {
            if (value1 > value2) return Trend.Bullish;
            if (value1 < value2) return Trend.Bearish;
            return Trend.NonTrended;
        }
    }
}
