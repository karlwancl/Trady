using System.Collections.Generic;
using Trady.Analysis.Infrastructure;
using Trady.Analysis.Pattern.State;

namespace Trady.Analysis.Pattern.Indicator
{
    public partial class StochasticsOvertrade
    {
        public abstract class IndicatorBase : AnalyzableBase<(decimal High, decimal Low, decimal Close), Overtrade?>
        {
            private AnalyzableBase<(decimal High, decimal Low, decimal Close), (decimal? K, decimal? D, decimal? J)> _sto;

            public IndicatorBase(IList<(decimal High, decimal Low, decimal Close)> inputs, AnalyzableBase<(decimal High, decimal Low, decimal Close), (decimal? K, decimal? D, decimal? J)> sto)
                : base(inputs)
            {
                _sto = sto;
            }

            protected override Overtrade? ComputeByIndexImpl(int index)
                => StateHelper.IsOvertrade(_sto[index].K);
        }
    }
}