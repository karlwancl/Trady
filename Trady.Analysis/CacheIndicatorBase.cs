using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Indicator.Helper;
using Trady.Core;

namespace Trady.Analysis
{
    public abstract class CacheIndicatorBase<TTick> : IndicatorBase<TTick> where TTick: ITick
    {
        private IMemoryCache _cache;
        private readonly Func<int, int?> _getPriorCacheAvailableIndex;

        public CacheIndicatorBase(Equity equity, params int[] parameters) : base(equity, parameters)
        {
            _cache = new MemoryCache(new MemoryCacheOptions());
            _getPriorCacheAvailableIndex = i =>
            {
                for (int j = i - 1; j >= 0; j--)
                {
                    if (_cache.TryGetValue(Equity[j].DateTime, out TTick tick))
                        return j;
                }
                return null;
            };
        }

        public sealed override TTick ComputeByIndex(int index)
        {
            if (_cache.TryGetValue(Equity[index].DateTime, out TTick tick))
                return tick;

            if (index < FirstValueIndex)
            {
                tick = ComputeNullValue(index);
                _cache.GetOrCreate(Equity[index].DateTime, entry => tick);
            }
            else if (index == FirstValueIndex)
            {
                tick = ComputeFirstValue(index);
                _cache.GetOrCreate(Equity[index].DateTime, entry => tick);
            }
            else
            {
                int startIndex = _getPriorCacheAvailableIndex(index) ?? 0;
                for (int i = startIndex; i < index; i++)
                {
                    bool canGetCached = _cache.TryGetValue(Equity[i].DateTime, out TTick prevTick);
                    if (!canGetCached)
                        prevTick = ComputeByIndex(i);
                    tick = ComputeIndexValue(i + 1, prevTick);
                    _cache.GetOrCreate(Equity[index].DateTime, entry => tick);
                }
            }
            return tick;
        }

        protected abstract int FirstValueIndex { get; }

        protected abstract TTick ComputeNullValue(int index);

        protected abstract TTick ComputeFirstValue(int index);

        protected abstract TTick ComputeIndexValue(int index, TTick prevTick);
    }
}
