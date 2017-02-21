using Trady.Analysis.Indicator;
using Trady.Analysis.Pattern.Helper;
using Trady.Core;

namespace Trady.Analysis.Pattern.Indicator
{
    public partial class StochasticsOvertrade
    {
        public abstract class IndicatorBase : IndicatorBase<MultistateResult<Overtrade?>>
        {
            private IndicatorBase<Stochastics.IndicatorResult> _stoIndicator;

            public IndicatorBase(Equity equity, IndicatorBase<Stochastics.IndicatorResult> stoIndicator)
                : base(equity, stoIndicator.Parameters)
            {
                _stoIndicator = stoIndicator;
            }

            protected override MultistateResult<Overtrade?> ComputeByIndexImpl(int index)
            {
                var result = _stoIndicator.ComputeByIndex(index);
                return new MultistateResult<Overtrade?>(Equity[index].DateTime, Decision.IsOvertrade(result.K));
            }
        }
    }
}