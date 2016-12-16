using System;
using System.Collections.Generic;
using System.Text;
using Trady.Analysis.Indicator;
using Trady.Analysis.Pattern.Helper;
using Trady.Core;

namespace Trady.Analysis.Pattern.Indicator
{
    public class OnBalanceVolumeTrend : IndicatorBase<MultistateResult<Trend?>>
    {
        private OnBalanceVolume _obvIndicator;

        public OnBalanceVolumeTrend(Equity equity) : base(equity)
        {
            _obvIndicator = new OnBalanceVolume(equity);
        }

        public override MultistateResult<Trend?> ComputeByIndex(int index)
        {
            if (index < 1)
                return new MultistateResult<Trend?>(Equity[index].DateTime, null);

            var latest = _obvIndicator.ComputeByIndex(index);
            var secondLatest = _obvIndicator.ComputeByIndex(index - 1);

            return new MultistateResult<Trend?>(Equity[index].DateTime, 
                Decision.IsTrending(latest.Obv - secondLatest.Obv));
        }
    }
}
