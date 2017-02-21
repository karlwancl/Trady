using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using Trady.Core;

namespace Trady.Analysis
{
    public static class AnalyzableLocator
    {
        private static IMemoryCache _cache = new MemoryCache(new MemoryCacheOptions());

        private static MemoryCacheEntryOptions _policy = new MemoryCacheEntryOptions
        {
            SlidingExpiration = TimeSpan.FromMinutes(15)
        };

        public static TAnalytic GetOrCreateAnalytic<TAnalytic>(this Equity equity, params object[] parameters)
            where TAnalytic : IAnalyzable
        {
            string key = $"{equity.GetHashCode()}#{typeof(TAnalytic).Name}#{string.Join("|", parameters)}";
            if (!_cache.TryGetValue(key, out TAnalytic output))
            {
                var paramsList = new List<object>();
                paramsList.Add(equity);
                paramsList.AddRange(parameters);
                output = _cache.Set(key, (TAnalytic)Activator.CreateInstance(typeof(TAnalytic), paramsList.ToArray()), _policy);
            }
            return output;
        }
    }
}