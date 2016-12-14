using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Trady.Analysis.Pattern.Indicator;
using Trady.Core;

namespace Trady.Analysis
{
    public static class AnalyticLocator
    {
        private static IMemoryCache _cache = new MemoryCache(new MemoryCacheOptions());
        private static MemoryCacheEntryOptions _policy = new MemoryCacheEntryOptions
        {
            SlidingExpiration = TimeSpan.FromMinutes(10)
        };

        public static TAnalytic GetOrCreateAnayltic<TAnalytic>(this Equity equity, params int[] parameters) 
            where TAnalytic: IAnalytic
        {
            string key = $"{equity.GetHashCode()}#{typeof(TAnalytic).Name}#{string.Join("|", parameters)}";
            bool isGetCached = _cache.TryGetValue(key, out TAnalytic output);
            if (!isGetCached)
            { 
                var paramsList = new List<object> { };
                paramsList.Add(equity);
                paramsList.AddRange(parameters.Select(p => (object)p));
                output = _cache.Set(key, (TAnalytic)Activator.CreateInstance(typeof(TAnalytic), paramsList.ToArray()), _policy);
            }
            return output;
        }
    }
}
