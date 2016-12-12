using Trady.Analysis.Indicator;
using Trady.Analysis.Pattern.Helper;
using Trady.Core;

namespace Trady.Analysis.Pattern.Indicator
{
    public class BollingerBandsInRange : AnalyticBase<MultistateResult<Overboundary?>>
    {
        private BollingerBands _bbIndicator;

        public BollingerBandsInRange(Equity equity, int periodCount, int sdCount) : base(equity)
        {
            _bbIndicator = new BollingerBands(equity, periodCount, sdCount);
        }

        public override MultistateResult<Overboundary?> ComputeByIndex(int index)
        {
            var result = _bbIndicator.ComputeByIndex(index);
            var current = Equity[index];

            return new MultistateResult<Overboundary?>(current.DateTime, 
                ResultExt.IsOverbound(current.Close, result.Lower, result.Upper));
        }
    }
}
