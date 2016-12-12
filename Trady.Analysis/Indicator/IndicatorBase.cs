using Trady.Core;

namespace Trady.Analysis.Indicator
{
    public abstract class IndicatorBase<TTick> : AnalyticBase<TTick> where TTick: ITick
    {
        public IndicatorBase(Equity equity, params int[] parameters) : base(equity)
        {
            Parameters = parameters;
        }

        protected int[] Parameters { get; private set; }
    }
}
