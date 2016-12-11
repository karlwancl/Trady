using Trady.Analysis.Indicator;
using Trady.Core;

namespace Trady.Analysis.Pattern.Indicator
{
    public class BollingerBandsInRange : PatternBase<MultistateResult<Overboundary>>
    {
        private BollingerBands _bbIndicator;

        public BollingerBandsInRange(Equity equity, int periodCount, int sdCount) : base(equity)
        {
            _bbIndicator = new BollingerBands(equity, periodCount, sdCount);
        }

        protected override TickBase ComputeResultByIndex(int index)
        {
            var result = _bbIndicator.ComputeByIndex(index);
            var current = Equity[index];
            return new MultistateResult<Overboundary>(current.DateTime, GetOverboundary(current.Close, result.Lower, result.Upper));
        }

        private Overboundary GetOverboundary(decimal value, decimal lower, decimal upper)
        {
            if (value <= lower) return Overboundary.BelowLower;
            if (value >= upper) return Overboundary.AboveUpper;
            return Overboundary.InRange;
        }
    }
}
