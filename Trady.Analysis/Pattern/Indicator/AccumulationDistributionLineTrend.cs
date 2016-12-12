using System;
using System.Collections.Generic;
using System.Text;
using Trady.Analysis.Indicator;
using Trady.Analysis.Pattern.Helper;
using Trady.Core;

namespace Trady.Analysis.Pattern.Indicator
{
    public class AccumulationDistributionLineTrend : AnalyticBase<MultistateResult<Trend?>>
    {
        private AccumulationDistributionLine _accumDistIndicator;

        public AccumulationDistributionLineTrend(Equity series) : base(series)
        {
            _accumDistIndicator = new AccumulationDistributionLine(series);
        }

        public override MultistateResult<Trend?> ComputeByIndex(int index)
        {
            if (index < 1)
                return new MultistateResult<Trend?>(Equity[index].DateTime, null);

            var latest = _accumDistIndicator.ComputeByIndex(index);
            var secondLatest = _accumDistIndicator.ComputeByIndex(index - 1);

            return new MultistateResult<Trend?>(Equity[index].DateTime, 
                ResultExt.IsTrending(latest.AccumDist - secondLatest.AccumDist));
        }
    }
}
