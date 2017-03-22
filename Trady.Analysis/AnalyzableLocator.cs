using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Trady.Core;
using Trady.Core.Infrastructure;

namespace Trady.Analysis
{
    public static class AnalyzableLocator
    {
        private static IMemoryCache _cache = new MemoryCache(new MemoryCacheOptions());

        private static MemoryCacheEntryOptions _policy = new MemoryCacheEntryOptions
        {
            SlidingExpiration = TimeSpan.FromMinutes(60)
        };

        public static object GetOrCreateAnalytic(this Equity equity, Type analyticType, params object[] parameters)
        {
            if (!typeof(IAnalyzable).IsAssignableFrom(analyticType))
                throw new ArgumentException($"{analyticType.Name} is not a valid object to create");

            string key = $"{equity.GetHashCode()}#{analyticType.Name}#{string.Join("|", parameters)}";
            if (!_cache.TryGetValue(key, out object output))
            {
                var paramsList = new List<object>();
                paramsList.Add(equity);
                paramsList.AddRange(parameters);
                output = _cache.Set(key, Activator.CreateInstance(analyticType, paramsList.ToArray()), _policy);
            }
            return output;
        }

        public static TAnalytic GetOrCreateAnalytic<TAnalytic>(this Equity equity, params object[] parameters)
            where TAnalytic : IAnalyzable
            => (TAnalytic)GetOrCreateAnalytic(equity, typeof(TAnalytic), parameters);

        public static async Task<object> GetOrCreateAnalyticWithTickProviderAsync(this Equity equity, Type analyticType, ITickProvider provider, params object[] parameters)
        {
            if (!typeof(IAnalyzable).IsAssignableFrom(analyticType))
                throw new ArgumentException($"{analyticType.Name} is not a valid object to create");

            string key = $"{equity.GetHashCode()}#{analyticType.Name}#{string.Join("|", parameters)}#{provider.GetHashCode()}";
            if (!_cache.TryGetValue(key, out object output))
            {
                var paramsList = new List<object>();
                paramsList.Add(equity);
                paramsList.AddRange(parameters);
                var instance = (IAnalyzable)Activator.CreateInstance(analyticType, paramsList.ToArray());
                await instance.InitWithTickProviderAsync(provider);
                output = _cache.Set(key, instance, _policy);
            }
            return output;
        }

        public static async Task<TAnalytic> GetOrCreateAnalyticWithTickProviderAsync<TAnalytic>(this Equity equity, ITickProvider provider, params object[] parameters)
            where TAnalytic : IAnalyzable
            => (TAnalytic)await GetOrCreateAnalyticWithTickProviderAsync(equity, typeof(TAnalytic), provider, parameters);
    }
}