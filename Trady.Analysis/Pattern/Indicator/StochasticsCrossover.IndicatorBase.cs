using System.Collections.Generic;
using Trady.Analysis.Infrastructure;
using Trady.Analysis.Pattern.State;

namespace Trady.Analysis.Pattern.Indicator
{
    public partial class StochasticsCrossover
    {
        public abstract class IndicatorBase : AnalyzableBase<(decimal High, decimal Low, decimal Close), Crossover?>
        {
            private AnalyzableBase<(decimal High, decimal Low, decimal Close), (decimal? K, decimal? D, decimal? J)> _sto;

            protected IndicatorBase(IList<(decimal High, decimal Low, decimal Close)> inputs, AnalyzableBase<(decimal High, decimal Low, decimal Close), (decimal? K, decimal? D, decimal? J)> sto)
                : base(inputs)
            {
                _sto = sto;
            }

            protected override Crossover? ComputeByIndexImpl(int index)
            {
                if (index < 1)
                    return null;

                var latest = _sto[index];
                var secondLatest = _sto[index - 1];

                var latestKdOsc = latest.K - latest.D;
                var secondLatestKsOsc = secondLatest.K - secondLatest.D;

                return StateHelper.IsCrossover(latestKdOsc, secondLatestKsOsc);
            }
        }
    }
}