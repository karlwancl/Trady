using Trady.Analysis.Indicator;
using Trady.Analysis.Pattern.Helper;
using Trady.Core;

namespace Trady.Analysis.Pattern.Indicator
{
    public class BollingerBandsInRange : IndicatorBase<MultistateResult<Overboundary?>>
    {
        private BollingerBands _bbIndicator;

        public BollingerBandsInRange(Equity equity, int periodCount, int sdCount)
            : base(equity, periodCount, sdCount)
        {
            _bbIndicator = new BollingerBands(equity, periodCount, sdCount);
        }

        protected override MultistateResult<Overboundary?> ComputeByIndexImpl(int index)
        {
            var result = _bbIndicator.ComputeByIndex(index);
            var current = Equity[index];

            return new MultistateResult<Overboundary?>(current.DateTime,
                Decision.IsOverbound(current.Close, result.Lower, result.Upper));
        }
    }
}