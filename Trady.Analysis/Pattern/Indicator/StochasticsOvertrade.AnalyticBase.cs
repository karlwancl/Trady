using Trady.Analysis.Indicator;
using Trady.Analysis.Pattern.Helper;
using Trady.Core;

namespace Trady.Analysis.Pattern.Indicator
{
    public partial class StochasticsOvertrade
    {
        public abstract class AnalyticBase : AnalyticBase<MultistateResult<Overtrade?>>
        {
            private IndicatorBase<Stochastics.IndicatorResult> _stoIndicator;

            public AnalyticBase(Equity equity, IndicatorBase<Stochastics.IndicatorResult> stoIndicator) : base(equity)
            {
                _stoIndicator = stoIndicator;
            }

            public override MultistateResult<Overtrade?> ComputeByIndex(int index)
            {
                var result = _stoIndicator.ComputeByIndex(index);
                return new MultistateResult<Overtrade?>(Equity[index].DateTime, ResultExt.IsOvertrade(result.K));
            }
        }
    }
}
