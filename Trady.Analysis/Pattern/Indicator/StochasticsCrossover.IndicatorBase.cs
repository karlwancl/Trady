using System;
using System.Collections.Generic;
using Trady.Analysis.Infrastructure;
using Trady.Analysis.Pattern.State;

namespace Trady.Analysis.Pattern.Indicator
{
    public partial class StochasticsCrossover
    {
        public abstract class IndicatorBase<TInput, TOutput> : AnalyzableBase<TInput, (decimal High, decimal Low, decimal Close), Crossover?, TOutput>
        {
            readonly AnalyzableBase<(decimal High, decimal Low, decimal Close), (decimal High, decimal Low, decimal Close), (decimal? K, decimal? D, decimal? J), (decimal? K, decimal? D, decimal? J)> _sto;

            protected IndicatorBase(
                IEnumerable<TInput> inputs,
                Func<TInput, (decimal High, decimal Low, decimal Close)> inputMapper,
                Func<TInput, Crossover?, TOutput> outputMapper,
                AnalyzableBase<(decimal High, decimal Low, decimal Close), (decimal High, decimal Low, decimal Close), (decimal? K, decimal? D, decimal? J), (decimal? K, decimal? D, decimal? J)> sto)
                : base(inputs, inputMapper, outputMapper)
            {
                _sto = sto;
            }

            protected override Crossover? ComputeByIndexImpl(IEnumerable<(decimal High, decimal Low, decimal Close)> mappedInputs, int index)
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