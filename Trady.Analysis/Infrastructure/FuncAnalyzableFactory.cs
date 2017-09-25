using System;
using System.Collections.Generic;
using Trady.Analysis.Indicator;
using Trady.Core.Infrastructure;
using Trady.Core;
using System.Linq;

namespace Trady.Analysis.Infrastructure
{
    internal static class FuncAnalyzableFactory
    {
        public static IFuncAnalyzable<TOutput> CreateAnalyzable<TInput, TOutput>(string name, IEnumerable<TInput> inputs, params decimal[] parameters)
        {
            var func = (Func<IReadOnlyList<TInput>, int, IReadOnlyList<decimal>, IAnalyzeContext<TInput>, decimal?>)FuncRegistry.Get(name);
            return new FuncAnalyzable<TInput, TOutput>(inputs, parameters).Init(func);
        }

        public static IFuncAnalyzable<AnalyzableTick<decimal?>> CreateAnalyzable(string name, IEnumerable<Candle> candles, params decimal[] parameters)
            => CreateAnalyzable<Candle, AnalyzableTick<decimal?>>(name, candles, parameters);
	}
}
