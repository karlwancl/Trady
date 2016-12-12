using System;
using System.Collections.Generic;
using System.Text;
using Trady.Analysis.Indicator;
using Trady.Analysis.Pattern.Helper;
using Trady.Core;

namespace Trady.Analysis.Pattern.Indicator
{
    public class RelativeStrengthIndexOvertrade : AnalyticBase<MultistateResult<SevereOvertrade?>>
    {
        private RelativeStrengthIndex _rsiIndicator;

        public RelativeStrengthIndexOvertrade(Equity equity, int periodCount) : base(equity)
        {
            _rsiIndicator = new RelativeStrengthIndex(equity, periodCount);
        }

        public override MultistateResult<SevereOvertrade?> ComputeByIndex(int index)
        {
            var result = _rsiIndicator.ComputeByIndex(index);

            return new MultistateResult<SevereOvertrade?>(Equity[index].DateTime, 
                ResultExt.IsSeverelyOvertrade(result.Rsi));
        }
    }
}
