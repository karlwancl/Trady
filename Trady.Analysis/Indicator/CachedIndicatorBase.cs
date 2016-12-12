using System;
using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Indicator.Helper;
using Trady.Core;

namespace Trady.Analysis.Indicator
{
    public abstract class CachedIndicatorBase<TTick> : IndicatorBase<TTick> where TTick: ITick
    {
        private const int MaxStackCount = 192;
        private int _stackCount;

        private Cache<TTick> _cache;

        protected CachedIndicatorBase(Equity equity, params int[] parameters) : base(equity, parameters)
        {
            _stackCount = 0;
            _cache = new Cache<TTick>();
        }

        protected abstract Func<int, TTick> FirstValueFunction { get; }

        public sealed override TTick ComputeByIndex(int index)
        {
            var dateTime = Equity[index].DateTime;
            var item = _cache.GetFromCacheOrDefault(dateTime);
            if (item == null)
            {
                if (_stackCount < MaxStackCount)
                {
                    _stackCount++;
                    item = ComputeByIndexUncached(index);
                }
                else
                {
                    item = FirstValueFunction(index);
                    _stackCount = 0;
                }
            }
            _cache.AddToCache(item);
            return item;
        }

        protected abstract TTick ComputeByIndexUncached(int index);
    }
}
