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

        public static IFuncAnalyzable<IAnalyzableTick<decimal?>> CreateAnalyzable(string name, IEnumerable<IOhlcv> candles, params decimal[] parameters)
            => CreateAnalyzable<IOhlcv, IAnalyzableTick<decimal?>>(name, candles, parameters);
	}
}
