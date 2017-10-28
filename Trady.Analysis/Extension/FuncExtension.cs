using System;
using System.Collections.Generic;

using Trady.Analysis.Indicator;
using Trady.Core.Infrastructure;

namespace Trady.Analysis.Extension
{
    public static class FuncExtension
    {
        public static FuncAnalyzable<IOhlcvData, AnalyzableTick<decimal?>> AsAnalyzable(this Func<IReadOnlyList<IOhlcvData>, int, IReadOnlyList<decimal>, IAnalyzeContext<IOhlcvData>, decimal?> func, IEnumerable<IOhlcvData> inputs, params decimal[] parameters)
            => new FuncAnalyzable(inputs, parameters).Init(func);

        public static FuncAnalyzable<TInput, decimal?> AsAnalyzable<TInput>(this Func<IReadOnlyList<TInput>, int, IReadOnlyList<decimal> ,IAnalyzeContext<TInput>, decimal?> func, IEnumerable<TInput> inputs, params decimal[] parameters)
	        => new FuncAnalyzable<TInput, decimal?>(inputs, parameters).Init(func);
    }
}
