using System;
using System.Collections.Generic;
using Trady.Analysis.Indicator;
using Trady.Analysis.Infrastructure;
using Trady.Core;
using Trady.Core.Infrastructure;

namespace Trady.Analysis
{
    public static class FuncExtension
    {
        public static FuncAnalyzable<Candle, AnalyzableTick<decimal?>> AsAnalyzable(this Func<IReadOnlyList<Candle>, int, decimal?> func, IEnumerable<Candle> inputs)
            => new FuncAnalyzable(inputs).Init(func);

        public static FuncAnalyzable<TInput, decimal?> AsAnalyzable<TInput>(this Func<IReadOnlyList<TInput>, int, decimal?> func, IEnumerable<TInput> inputs)
	        => new FuncAnalyzable<TInput, decimal?>(inputs).Init(func);

        public static IEnumerable<TInput> AsEnumerable<TInput>(this Func<int, TInput> func) => new FuncEnumerable<TInput>(func);
    }
}
