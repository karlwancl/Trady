using System;
using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Helper;
using Trady.Core.Infrastructure;

namespace Trady.Analysis.Infrastructure
{
    public abstract class AnalyzableBase<TInput, TOutput> : IAnalyzable<TInput, TOutput>
    {
        protected IDictionary<int, TOutput> _cache;

        public AnalyzableBase(IList<TInput> inputs)
        {
            _cache = new Dictionary<int, TOutput>();
            Inputs = inputs;
        }

        public IList<TInput> Inputs { get; }

        IList<object> IAnalyzable.Inputs => Inputs.Select(i => (object)i).ToList();

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

        protected virtual int GetComputeEndIndex(int? endIndex) => endIndex ?? Inputs.Count - 1;

        public TOutput ComputeByIndex(int index)
        {
            if (_cache.TryGetValue(index, out TOutput value))
                return value;

            value = ComputeByIndexImpl(index);
            _cache.AddOrUpdate(index, value);
            return value;
        }

        protected abstract TOutput ComputeByIndexImpl(int index);

        object IAnalyzable.ComputeByIndex(int index) => ComputeByIndex(index);

        public IList<TOutput> Compute() => Compute(null, null);
    }
}