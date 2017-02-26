using Trady.Analysis.Infrastructure;
using Trady.Core;
using Trady.Core.Infrastructure;

namespace Trady.Analysis.Indicator
{
    public abstract class IndicatorBase<TTick> : AnalyzableBase<TTick>, IIndicator where TTick : ITick
    {
        public IndicatorBase(Equity equity, params int[] parameters) : base(equity)
        {
            Parameters = parameters;
        }

        public int[] Parameters { get; private set; }
    }
}