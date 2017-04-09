using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Helper;

namespace Trady.Analysis.Infrastructure
{
    public abstract class CummulativeAnalyzableBase<TInput, TOutput> : AnalyzableBase<TInput, TOutput>
    {
        public CummulativeAnalyzableBase(IList<TInput> equity) : base(equity)
        {
        }

        protected sealed override TOutput ComputeByIndexImpl(int index)
        {
            TOutput tick = default(TOutput);
            if (index < InitialValueIndex)
                tick = ComputeNullValue(index);
            else if (index == InitialValueIndex)
                tick = ComputeInitialValue(index);
            else
            {
                int idx = _cache.Select(kvp => kvp.Key).Where(k => k >= InitialValueIndex).DefaultIfEmpty(InitialValueIndex).Max();
                for (int i = idx; i < index; i++)
                {
                    var prevTick = ComputeByIndex(i);
                    tick = ComputeCummulativeValue(i + 1, prevTick);
                    _cache.AddOrUpdate(i + 1, tick);
                }
            }
            return tick;
        }

        protected abstract int InitialValueIndex { get; }

        protected abstract TOutput ComputeNullValue(int index);

        protected abstract TOutput ComputeInitialValue(int index);

        protected abstract TOutput ComputeCummulativeValue(int index, TOutput prevOutput);
    }
}