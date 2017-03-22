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
        private IDictionary<DateTime, TTick> _cache;

        public AnalyzableBase(Equity equity)
        {
            Equity = equity;
            _cache = new Dictionary<DateTime, TTick>();
        }

        /// <summary>
        /// Equity, time series of candles
        /// </summary>
        public Equity Equity { get; private set; }

        /// <summary>
        /// Compute indicator/pattern results by datetime range
        /// </summary>
        /// <param name="startTime">Start time</param>
        /// <param name="endTime">End time</param>
        /// <returns>Indicator/Pattern results</returns>
        public TimeSeries<TTick> Compute(DateTime? startTime, DateTime? endTime)
        {
            var ticks = new List<TTick>();

            int startIndex = ComputeStartIndex(startTime);
            int endIndex = ComputeEndIndex(endTime);

            for (int i = startIndex; i <= endIndex; i++)
                ticks.Add(ComputeByIndex(i));

            return new TimeSeries<TTick>(Equity.Name, ticks, Equity.Period);
        }

        /// <summary>
        /// Compute indicator/pattern results by datetime point
        /// </summary>
        /// <param name="dateTime">Datetime</param>
        /// <returns>Indicator/Pattern result</returns>
        public TTick ComputeByDateTime(DateTime dateTime)
        {
            int? index = Equity.FindLastCandleIndexOrDefault(c => c.DateTime <= dateTime);
            return index.HasValue ? ComputeByIndex(index.Value) : default(TTick);
        }

        /// <summary>
        /// Compute indicator/pattern result by index
        /// </summary>
        /// <param name="index">Index</param>
        /// <returns>Indicator/Pattern result</returns>
        public virtual TTick ComputeByIndex(int index)
        {
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

        /// <summary>
        /// Compute indicator/pattern result by index, plain implementation without caching/data retrieval process
        /// </summary>
        /// <param name="index">Index</param>
        /// <returns>Indicator/Pattern result</returns>
        protected abstract TTick ComputeByIndexImpl(int index);

        /// <summary>
        /// Compute start index by start time
        /// </summary>
        /// <param name="startTime">Start time</param>
        /// <returns>Index</returns>
        protected virtual int ComputeStartIndex(DateTime? startTime)
            => startTime.HasValue ? Equity.FindCandleIndexOrDefault(c => c.DateTime >= startTime) ?? 0 : 0;

        /// <summary>
        /// Compute end index by end time
        /// </summary>
        /// <param name="endTime">End time</param>
        /// <returns>Index</returns>
        protected virtual int ComputeEndIndex(DateTime? endTime)
            => endTime.HasValue ? Equity.FindLastCandleIndexOrDefault(c => c.DateTime < endTime) ?? Equity.Count - 1 : Equity.Count - 1;

        // TODO: Find ways to optimize the process of data retrieval from external data source, partial get or get all
        /// <summary>
        /// Initialize with tick provider
        /// </summary>
        /// <param name="provider">Tick provider</param>
        /// <returns>Task object</returns>
        public async Task InitWithTickProviderAsync(ITickProvider provider)
        {
            _provider = provider;

            var dependencies = GetType().GetFields(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.NonPublic)
                .Where(f => typeof(IAnalyzable).IsAssignableFrom(f.FieldType))
                .Select(f => (IAnalyzable)f.GetValue(this));

            foreach (var dependency in dependencies)
                await dependency.InitWithTickProviderAsync(_provider.Clone()).ConfigureAwait(false);

            await _provider.InitWithAnalyzableAsync(this).ConfigureAwait(false);

            if (_provider != null && _provider.IsReady)
            {
                var startTime = _cache.Any() ? _cache.Max(t => t.Key) : DateTime.MinValue;
                var ticks = await _provider.GetAsync<TTick>(startTime).ConfigureAwait(false);
                foreach (var tick in ticks.Where(t => !_cache.ContainsKey(t.DateTime)))
                    _cache.Add(tick.DateTime, tick);
            }
        }
    }
}