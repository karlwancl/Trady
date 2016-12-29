using Trady.Core;

namespace Trady.Analysis
{
    public abstract class IndicatorBase<TTick> : AnalyzableBase<TTick>, IIndicator where TTick: ITick
    {
        public IndicatorBase(Equity equity, params int[] parameters) : base(equity)
        {
            Parameters = parameters;
        }

        public int[] Parameters { get; private set; }
    }
}
