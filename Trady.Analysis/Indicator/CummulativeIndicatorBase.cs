using System;
using System.Collections.Generic;
using Trady.Analysis.Indicator.Helper;
using Trady.Core;
using Trady.Core.Infrastructure;

namespace Trady.Analysis.Indicator
{
    public abstract class CummulativeIndicatorBase<TTick> : IndicatorBase<TTick> where TTick : ITick
    {
        private IDictionary<DateTime, TTick> _cache;
        private readonly Func<int, int?> _getNearestCachedIndex;

        public CummulativeIndicatorBase(Equity equity, params int[] parameters) : base(equity, parameters)
        {
            _cache = new Dictionary<DateTime, TTick>();
            _getNearestCachedIndex = i =>
            {
                for (int j = i - 1; j >= 0; j--)
                    if (_cache.TryGetValue(Equity[j].DateTime, out TTick tick))
                        return j;
                return null;
            };
        }

        protected sealed override TTick ComputeByIndexImpl(int index)
        {
            if (_cache.TryGetValue(Equity[index].DateTime, out TTick tick))
                return tick;

            if (index < FirstValueIndex)
            {
                tick = ComputeNullValue(index);
                _cache.AddOrUpdate(Equity[index].DateTime, tick);
            }
            else if (index == FirstValueIndex)
            {
                tick = ComputeFirstValue(index);
                _cache.AddOrUpdate(Equity[index].DateTime, tick);
            }
            else
            {
                int startIndex = _getNearestCachedIndex(index) ?? 0;
                for (int i = startIndex; i < index; i++)
                {
                    TTick prevTick;
                    if (!_cache.TryGetValue(Equity[i].DateTime, out prevTick))
                        prevTick = ComputeByIndexImpl(i);
                    tick = ComputeIndexValue(i + 1, prevTick);
                    _cache.AddOrUpdate(Equity[index].DateTime, tick);
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