using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Trady.Core;
using Trady.Core.Helper;
using Trady.Core.Infrastructure;

namespace Trady.Analysis.Infrastructure
{
    public abstract class AnalyzableBase<TTick> : IAnalyzable<TTick> where TTick : ITick
    {
        private ITickProvider _provider;
        private IMemoryCache _cache;
        private DateTime _prevProviderInitTime;

        public AnalyzableBase(Equity equity)
        {
            Equity = equity;
        }

        public Equity Equity { get; private set; }

        public TimeSeries<TTick> Compute(DateTime? startTime, DateTime? endTime)
        {
            var ticks = new List<TTick>();

            int startIndex = ComputeStartIndex(startTime);
            int endIndex = ComputeEndIndex(endTime);

            for (int i = startIndex; i <= endIndex; i++)
                ticks.Add(ComputeByIndex(i));

            return new TimeSeries<TTick>(Equity.Name, ticks, Equity.Period, Equity.MaxTickCount);
        }

        public TTick ComputeByDateTime(DateTime dateTime)
        {
            int? index = Equity.FindLastCandleIndexOrDefault(c => c.DateTime <= dateTime);
            return index.HasValue ? ComputeByIndex(index.Value) : default(TTick);
        }

        public virtual TTick ComputeByIndex(int index)
        {
            if (_provider != null && DateTime.Now - _prevProviderInitTime >= new TimeSpan(0, 15, 0))
                InitWithTickProviderAsync(_provider).Wait();

            bool isProviderValid = _provider != null && _provider.IsReady;
            bool isIndexValid = index >= 0 && index <= Equity.Count - 1;
            if (isProviderValid && isIndexValid)
            {
                // Try get from cache first, update cache & check again if entry not found
                if (_cache.TryGetValue(Equity[index].DateTime, out TTick tick))
                    return tick;
                else
                {
                    InitWithTickProviderAsync(_provider).Wait();
                    if (_cache.TryGetValue(Equity[index].DateTime, out tick))
                        return tick;
                }
            }
            return ComputeByIndexImpl(index);
        }

        protected abstract TTick ComputeByIndexImpl(int index);

        protected virtual int ComputeStartIndex(DateTime? startTime)
            => startTime.HasValue ? Equity.FindCandleIndexOrDefault(c => c.DateTime >= startTime) ?? 0 : 0;

        protected virtual int ComputeEndIndex(DateTime? endTime)
            => endTime.HasValue ? Equity.FindLastCandleIndexOrDefault(c => c.DateTime < endTime) ?? Equity.Count - 1 : Equity.Count - 1;

        public async Task InitWithTickProviderAsync(ITickProvider provider)
        {
            _provider = provider;

            var dependencies = GetType().GetFields(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.NonPublic)
                .Where(f => typeof(IAnalyzable).IsAssignableFrom(f.FieldType))
                .Select(f => (IAnalyzable)f.GetValue(this));

            foreach (var dependency in dependencies)
                await dependency.InitWithTickProviderAsync(_provider.Clone());

            await _provider.InitWithAnalyzableAsync(this);

            _cache = new MemoryCache(new MemoryCacheOptions());
            if (_provider != null && _provider.IsReady)
            {
                var ticks = await _provider.GetAllAsync<TTick>().ConfigureAwait(false);
                ticks.ToList().ForEach(t => _cache.Set(t.DateTime, t));
            }

            _prevProviderInitTime = DateTime.Now;
        }
    }
}