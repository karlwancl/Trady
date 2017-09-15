using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Helper;

namespace Trady.Analysis.Infrastructure
{
    public abstract class CumulativeAnalyzableBase<TInput, TMappedInput, TOutputToMap, TOutput> : AnalyzableBase<TInput, TMappedInput, TOutputToMap, TOutput>
    {
        private ConcurrentDictionary<int, TOutputToMap> _cache;

        protected CumulativeAnalyzableBase(IEnumerable<TInput> inputs, Func<TInput, TMappedInput> inputMapper) : base(inputs, inputMapper)
        {
            _cache = new ConcurrentDictionary<int, TOutputToMap>();
        }

        protected sealed override TOutputToMap ComputeByIndexImpl(IReadOnlyList<TMappedInput> mappedInputs, int index)
        {
            if (_cache.TryGetValue(index, out TOutputToMap t))
                return t;

            var tick = default(TOutputToMap);
            if (index < InitialValueIndex)
                tick = ComputeNullValue(mappedInputs, index);
            else if (index == InitialValueIndex)
                tick = ComputeInitialValue(mappedInputs, index);
            else
            {
                // get start index of calculation to cache
                int cacheStartIndex = _cache.Keys.DefaultIfEmpty(InitialValueIndex).Where(k => k >= InitialValueIndex).Max();
                for (int i = cacheStartIndex; i < index; i++)
                {
                    var prevTick = _cache.GetOrAdd(i, _i => ComputeByIndexImpl(mappedInputs, _i));
                    tick = ComputeCumulativeValue(mappedInputs, i + 1, prevTick);
                    _cache.AddOrUpdate(i + 1, tick, (_i, _t) => tick);
                }
            }

            return tick;
        }

        protected virtual int InitialValueIndex { get; } = 0;

        protected virtual TOutputToMap ComputeNullValue(IReadOnlyList<TMappedInput> mappedInputs, int index) => default(TOutputToMap);

        protected abstract TOutputToMap ComputeInitialValue(IReadOnlyList<TMappedInput> mappedInputs, int index);

        protected abstract TOutputToMap ComputeCumulativeValue(IReadOnlyList<TMappedInput> mappedInputs, int index, TOutputToMap prevOutputToMap);
    }

    public abstract class CumulativeAnalyzableBase<TInput, TOutput> : CumulativeAnalyzableBase<TInput, TInput, TOutput, TOutput>
    {
        public CumulativeAnalyzableBase(IEnumerable<TInput> inputs)
            : base(inputs, i => i)
        {
        }
    }
}