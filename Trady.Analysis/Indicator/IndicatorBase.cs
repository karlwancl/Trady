using System.Collections.Generic;
using Trady.Analysis.Infrastructure;

namespace Trady.Analysis.Indicator
{
    public abstract class IndicatorBase<TInput, TOutput> : AnalyzableBase<TInput, TOutput>, IIndicator
    {
        public IndicatorBase(IList<TInput> inputs, params int[] parameters) : base(inputs)
        {
            Parameters = parameters;
        }

        public int[] Parameters { get; private set; }
    }
}