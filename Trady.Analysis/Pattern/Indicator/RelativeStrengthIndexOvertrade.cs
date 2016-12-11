using System;
using System.Collections.Generic;
using System.Text;
using Trady.Analysis.Indicator;
using Trady.Core;

namespace Trady.Analysis.Pattern.Indicator
{
    public class RelativeStrengthIndexOvertrade : PatternBase<MultistateResult<SevereOvertrade>>
    {
        private RelativeStrengthIndex _rsiIndicator;

        public RelativeStrengthIndexOvertrade(Equity equity, int periodCount) : base(equity)
        {
            _rsiIndicator = new RelativeStrengthIndex(equity, periodCount);
        }

        protected override TickBase ComputeResultByIndex(int index)
        {
            var result = _rsiIndicator.ComputeByIndex(index);
            return new MultistateResult<SevereOvertrade>(Equity[index].DateTime, GetOvertrade(result.Rsi));
        }

        private SevereOvertrade GetOvertrade(decimal value)
        {
            if (value <= 20) return SevereOvertrade.SevereOversold;
            if (value <= 30) return SevereOvertrade.Oversold;
            if (value >= 80) return SevereOvertrade.SevereOverbought;
            if (value >= 70) return SevereOvertrade.Overbought;
            return SevereOvertrade.Normal;
        }
    }
}
