using System;
using Trady.Core;
using static Trady.Analysis.Indicator.RelativeStrengthIndex;

namespace Trady.Analysis.Indicator
{
    public partial class RelativeStrengthIndex : IndicatorBase<IndicatorResult>
    {
        private RelativeStrength _rsIndicator;

        public RelativeStrengthIndex(Equity equity, int periodCount) : base(equity, periodCount)
        {
            _rsIndicator = new RelativeStrength(equity, periodCount);
        }

        public int PeriodCount => Parameters[0];

        public override IndicatorResult ComputeByIndex(int index)
        {
            var rsi = 100 - (100 / (1 + _rsIndicator.ComputeByIndex(index).Rs));
            return new IndicatorResult(Equity[index].DateTime, rsi);
        }
    }
}
