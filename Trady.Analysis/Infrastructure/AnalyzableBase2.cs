using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Trady.Analysis.Helper;
using Trady.Core.Infrastructure;

namespace Trady.Analysis.Infrastructure
{
    public abstract class AnalyzableBase2<TInput, TOutput> : IAnalyzable2<TInput, TOutput>
    {
        protected readonly Func<TInput, decimal> MappingFunction;
        protected IDictionary<int, TOutput> Cache;

        protected AnalyzableBase2(IEnumerable<TInput> inputs, Func<TInput, decimal> mappingFunction)
        {
            this.MappingFunction = mappingFunction;
            Cache = new Dictionary<int, TOutput>();
            Inputs = inputs;
        }

        public IEnumerable<TInput> Inputs { get; }

        public TOutput this[int index] => ComputeByIndex(index);

        public IList<TOutput> Compute(int? startIndex = null, int? endIndex = null)
        {
            var ticks = new List<TOutput>();

            int computedStartIndex = GetComputeStartIndex(startIndex);
            int computedEndIndex = GetComputeEndIndex(endIndex);

            for (int i = computedStartIndex; i <= computedEndIndex; i++)
                ticks.Add(ComputeByIndex(i));

            return ticks;
        }

        protected virtual int GetComputeStartIndex(int? startIndex) => startIndex ?? 0;

        protected virtual int GetComputeEndIndex(int? endIndex) => endIndex ?? Inputs.Count() - 1;

        public TOutput ComputeByIndex(int index)
        {
            if (Cache.TryGetValue(index, out TOutput value))
                return value;

            value = ComputeByIndexImpl(index);
            Cache.AddOrUpdate(index, value);
            return value;
        }

        protected abstract TOutput ComputeByIndexImpl(int index);

    }
}
