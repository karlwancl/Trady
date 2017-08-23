using System;
using System.Collections.Generic;
using Trady.Analysis.Infrastructure;
using Trady.Analysis.Pattern.State;

namespace Trady.Analysis.Pattern.Indicator
{
    public partial class StochasticsOvertrade
    {
        public abstract class IndicatorBase<TInput, TOutput> : AnalyzableBase<TInput, (decimal High, decimal Low, decimal Close), Overtrade?, TOutput>
        {
            readonly AnalyzableBase<(decimal High, decimal Low, decimal Close), (decimal High, decimal Low, decimal Close), (decimal? K, decimal? D, decimal? J), (decimal? K, decimal? D, decimal? J)> _sto;

            protected IndicatorBase(
                IEnumerable<TInput> inputs, 
                Func<TInput, (decimal High, decimal Low, decimal Close)> inputMapper, 
                AnalyzableBase<(decimal High, decimal Low, decimal Close), (decimal High, decimal Low, decimal Close), (decimal? K, decimal? D, decimal? J), (decimal? K, decimal? D, decimal? J)> sto) 
                : base(inputs, inputMapper)
            {
                _sto = sto;
            }

            protected override Overtrade? ComputeByIndexImpl(IEnumerable<(decimal High, decimal Low, decimal Close)> mappedInputs, int index)
			    => StateHelper.IsOvertrade(_sto[index].K);
		}
    }
}