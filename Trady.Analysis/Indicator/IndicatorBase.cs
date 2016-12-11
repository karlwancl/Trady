using Trady.Core;

namespace Trady.Analysis.Indicator
{
    public abstract class IndicatorBase : AnalyticBase
    {
        public IndicatorBase(Equity equity, params int[] parameters) : base(equity)
        {
            Parameters = parameters;
        }

        protected int[] Parameters { get; private set; }
    }
}
