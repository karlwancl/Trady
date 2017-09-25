using System;
using System.Collections.Generic;
using Trady.Analysis.Indicator;
using Trady.Core;
using Trady.Core.Infrastructure;

namespace Trady.Analysis
{
    public static class FuncExtension
    {
        public static FuncAnalyzable<Candle, AnalyzableTick<decimal?>> AsAnalyzable(this Func<IReadOnlyList<Candle>, int, IReadOnlyList<decimal>, IAnalyzeContext<Candle>, decimal?> func, IEnumerable<Candle> inputs, params decimal[] parameters)
            => new FuncAnalyzable(inputs, parameters).Init(func);

        public static FuncAnalyzable<TInput, decimal?> AsAnalyzable<TInput>(this Func<IReadOnlyList<TInput>, int, IReadOnlyList<decimal> ,IAnalyzeContext<TInput>, decimal?> func, IEnumerable<TInput> inputs, params decimal[] parameters)
	        => new FuncAnalyzable<TInput, decimal?>(inputs, parameters).Init(func);
    }
}
