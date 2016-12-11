using System;
using System.Collections.Generic;
using System.Text;
using Trady.Analysis.Indicator;
using Trady.Core;

namespace Trady.Analysis.Pattern.Indicator
{
    public class AccumulationDistributionLineTrend : PatternBase<MultistateResult<Trend>>
    {
        private AccumulationDistributionLine _accumDistIndicator;

        public AccumulationDistributionLineTrend(Equity series) : base(series)
        {
            _accumDistIndicator = new AccumulationDistributionLine(series);
        }

        protected override TickBase ComputeResultByIndex(int index)
        {
            if (index < 1)
                return new IsMatchedResult(Equity[index].DateTime, false);

            var latest = _accumDistIndicator.ComputeByIndex(index);
            var secondLatest = _accumDistIndicator.ComputeByIndex(index - 1);
            return new MultistateResult<Trend>(Equity[index].DateTime, GetTrend(latest.AccumDist, secondLatest.AccumDist));
        }

        private Trend GetTrend(decimal value1, decimal value2)
        {
            if (value1 > value2) return Trend.Bullish;
            if (value1 < value2) return Trend.Bearish;
            return Trend.NonTrended;
        }
    }
}
