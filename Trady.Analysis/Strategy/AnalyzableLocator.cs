using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Trady.Core;
using Trady.Core.Infrastructure;

namespace Trady.Analysis.Strategy
{
    static class AnalyzableLocator
    {
        static IMemoryCache _cache = new MemoryCache(new MemoryCacheOptions());

        static MemoryCacheEntryOptions _policy = new MemoryCacheEntryOptions
        {
            SlidingExpiration = TimeSpan.FromMinutes(1)
        };

        public static TAnalyzable GetOrCreateAnalyzable<TAnalyzable>(this IEnumerable<Candle> candles, params object[] parameters)
            where TAnalyzable : IAnalyzable
        {
            string key = $"{candles.GetHashCode()}#{typeof(TAnalyzable).Name}#{string.Join("|", parameters)}";
            if (!_cache.TryGetValue(key, out TAnalyzable output))
            {
                var paramsList = new List<object>();
                paramsList.Add(candles);
                paramsList.AddRange(parameters);

                // Get the default constructor for instantiation
                var ctor = typeof(TAnalyzable).GetConstructors(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance)
                    .FirstOrDefault(c => c.GetParameters().Any() && typeof(IEnumerable<Candle>).Equals(c.GetParameters().First().ParameterType));

                if (ctor == null)
                    throw new TargetInvocationException("Can't find default constructor for instantiation, please make sure that the analyzable has a constructor with IList<Candle> as the first parameter",
                        new ArgumentNullException(nameof(ctor)));

                output = _cache.Set(key, (TAnalyzable)ctor.Invoke(paramsList.ToArray()), _policy);
            }
            return output;
        }
    }
}