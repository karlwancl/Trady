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
        public static IFuncAnalyzable<TOutput> CreateAnalyzable<TInput, TOutput>(string name, IEnumerable<TInput> inputs, params object[] parameters)
        {
            var func = FuncRegistry.Get(name);
            var p = parameters.Cast<decimal>().ToArray();
            switch (func)
            {
                case Func<IReadOnlyList<TInput>, int, IAnalyzeContext<TInput>, decimal?> f:
                    return new Func0Analyzable<TInput, TOutput>(inputs).Init(f);
                case Func<IReadOnlyList<TInput>, int, decimal, IAnalyzeContext<TInput>, decimal?> f:
                    return new Func1Analyzable<TInput, TOutput>(inputs, p[0]).Init(f);
                case Func<IReadOnlyList<TInput>, int, decimal, decimal, IAnalyzeContext<TInput>, decimal?> f:
                    return new Func2Analyzable<TInput, TOutput>(inputs, p[0], p[1]).Init(f);
                case Func<IReadOnlyList<TInput>, int, decimal, decimal, decimal, IAnalyzeContext<TInput>, decimal?> f:
                    return new Func3Analyzable<TInput, TOutput>(inputs, p[0], p[1], p[2]).Init(f);
                default:
                    throw new TypeLoadException("Fail to cast to suitable func analyzable");
            }
        }

        public static IFuncAnalyzable<AnalyzableTick<decimal?>> CreateAnalyzable(string name, IEnumerable<Candle> candles, params object[] parameters)
            => CreateAnalyzable<Candle, AnalyzableTick<decimal?>>(name, candles, parameters);
	}
}
