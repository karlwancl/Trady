using System;
using System.Collections.Generic;
using Trady.Analysis.Indicator;
using Trady.Core;
using Trady.Core.Infrastructure;

namespace Trady.Analysis
{
    public static class FuncExtension
    {
        public static Func0Analyzable<Candle, AnalyzableTick<decimal?>> AsAnalyzable(this Func<IReadOnlyList<Candle>, int, IAnalyzeContext<Candle>, decimal?> func, IEnumerable<Candle> inputs)
            => new Func0Analyzable(inputs).Init(func);

        public static Func0Analyzable<TInput, decimal?> AsAnalyzable<TInput>(this Func<IReadOnlyList<TInput>, int, IAnalyzeContext<TInput>, decimal?> func, IEnumerable<TInput> inputs)
	        => new Func0Analyzable<TInput, decimal?>(inputs).Init(func);

		public static Func1Analyzable<Candle, AnalyzableTick<decimal?>> AsAnalyzable(this Func<IReadOnlyList<Candle>, int, decimal, IAnalyzeContext<Candle>, decimal?> func, IEnumerable<Candle> inputs, decimal parameter)
	        => new Func1Analyzable(inputs, parameter).Init(func);

		public static Func1Analyzable<TInput, decimal?> AsAnalyzable<TInput>(this Func<IReadOnlyList<TInput>, int, decimal, IAnalyzeContext<TInput>, decimal?> func, IEnumerable<TInput> inputs, decimal parameter)
			=> new Func1Analyzable<TInput, decimal?>(inputs, parameter).Init(func);

		public static Func2Analyzable<Candle, AnalyzableTick<decimal?>> AsAnalyzable(this Func<IReadOnlyList<Candle>, int, decimal, decimal, IAnalyzeContext<Candle>, decimal?> func, IEnumerable<Candle> inputs, decimal paremeter0, decimal parameter1)
	        => new Func2Analyzable(inputs, paremeter0, parameter1).Init(func);

		public static Func2Analyzable<TInput, decimal?> AsAnalyzable<TInput>(this Func<IReadOnlyList<TInput>, int, decimal ,decimal, IAnalyzeContext<TInput>, decimal?> func, IEnumerable<TInput> inputs, decimal parameter0, decimal parameter1)
			=> new Func2Analyzable<TInput, decimal?>(inputs, parameter0, parameter1).Init(func);

		public static Func3Analyzable<Candle, AnalyzableTick<decimal?>> AsAnalyzable(this Func<IReadOnlyList<Candle>, int, decimal, decimal, decimal, IAnalyzeContext<Candle>, decimal?> func, IEnumerable<Candle> inputs, decimal parameter0, decimal parameter1, decimal parameter2)
	        => new Func3Analyzable(inputs, parameter0, parameter1, parameter2).Init(func);

		public static Func3Analyzable<TInput, decimal?> AsAnalyzable<TInput>(this Func<IReadOnlyList<TInput>, int, decimal, decimal, decimal, IAnalyzeContext<TInput>, decimal?> func, IEnumerable<TInput> inputs, decimal parameter0, decimal parameter1, decimal parameter2)
			=> new Func3Analyzable<TInput, decimal?>(inputs, parameter0, parameter1, parameter2).Init(func);
    }
}
