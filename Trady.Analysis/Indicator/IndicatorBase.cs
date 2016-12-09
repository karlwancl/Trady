using Trady.Core;

namespace Trady.Analysis.Indicator
{
    public abstract class IndicatorBase : AnalyticBase<decimal>
    {
        protected IndicatorBase(Equity series, params int[] parameters) : base(series)
        {
            Parameters = parameters;
        }

        protected int[] Parameters { get; private set; }
    }
}
