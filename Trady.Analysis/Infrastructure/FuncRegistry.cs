using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using Trady.Analysis.Indicator;
using Trady.Core;
using Trady.Core.Infrastructure;

namespace Trady.Analysis.Infrastructure
{
    public static class FuncRegistry
    {
        static readonly ScriptOptions options = ScriptOptions.Default
                                                    .WithReferences(typeof(AnalyzeContext).GetTypeInfo().Assembly)
                                                    .WithImports(typeof(AnalyzeContext).Namespace, typeof(SimpleMovingAverage).Namespace);

        static readonly ConcurrentDictionary<string, object> _funcDict = new ConcurrentDictionary<string, object>();

		#region Func0

		public class Func0Globals<TInput>
		{
			public IReadOnlyList<TInput> c;
			public int i;
			public IAnalyzeContext<TInput> ctx;
		}

		private static Func<IReadOnlyList<TInput>, int, IAnalyzeContext<TInput>, decimal?> CreateFunc0<TInput>(string expression)
		{
			var @delegate = CSharpScript.Create<decimal?>(expression, options, typeof(Func0Globals<TInput>)).CreateDelegate();
			return (_c, _i, _ctx) => @delegate(new Func0Globals<TInput> { c = _c, i = _i, ctx = _ctx }).Result;
		}

		#endregion

		#region Func1

		public class Func1Globals<TInput> : Func0Globals<TInput>
		{
			public decimal p0;
		}

		private static Func<IReadOnlyList<TInput>, int, decimal, IAnalyzeContext<TInput>, decimal?> CreateFunc1<TInput>(string expression)
		{
			var @delegate = CSharpScript.Create<decimal?>(expression, options, typeof(Func1Globals<TInput>)).CreateDelegate();
			return (_c, _i, _p0, _ctx) => @delegate(new Func1Globals<TInput> { c = _c, i = _i, p0 = _p0, ctx = _ctx }).Result;
		}

		#endregion

		#region Func1

		public class Func2Globals<TInput> : Func1Globals<TInput>
		{
			public decimal p1;
		}

		private static Func<IReadOnlyList<TInput>, int, decimal, decimal, IAnalyzeContext<TInput>, decimal?> CreateFunc2<TInput>(string expression)
		{
			var @delegate = CSharpScript.Create<decimal?>(expression, options, typeof(Func2Globals<TInput>)).CreateDelegate();
			return (_c, _i, _p0, _p1, _ctx) => @delegate(new Func2Globals<TInput> { c = _c, i = _i, p0 = _p0, p1 = _p1, ctx = _ctx }).Result;
		}

		#endregion

		#region Func3

		public class Func3Globals<TInput> : Func2Globals<TInput>
		{
			public decimal p2;
		}

		private static Func<IReadOnlyList<TInput>, int, decimal, decimal, decimal, IAnalyzeContext<TInput>, decimal?> CreateFunc3<TInput>(string expression)
		{
			var @delegate = CSharpScript.Create<decimal?>(expression, options, typeof(Func3Globals<TInput>)).CreateDelegate();
			return (_c, _i, _p0, _p1, _p2, _ctx) => @delegate(new Func3Globals<TInput> { c = _c, i = _i, p0 = _p0, p1 = _p1, p2 = _p2, ctx = _ctx }).Result;
		}

		#endregion

        public static bool Register<TInput>(string name, string expression, bool @override = false)
		{
            if (@override)
                _funcDict.TryRemove(name, out _);

			if (expression.Contains("p2"))
				return _funcDict.TryAdd(name, CreateFunc3<TInput>(expression));
			else if (expression.Contains("p1"))
				return _funcDict.TryAdd(name, CreateFunc2<TInput>(expression));
			else if (expression.Contains("p0"))
				return _funcDict.TryAdd(name, CreateFunc1<TInput>(expression));
			else
				return _funcDict.TryAdd(name, CreateFunc0<TInput>(expression));
		}

        public static bool Register<TInput>(string name, Expression<Func<IReadOnlyList<TInput>, int, IAnalyzeContext<TInput>, decimal?>> expr, bool @override = false)
            => Register<TInput>(name, expr.Body.ToString(), @override);

		public static bool Register<TInput>(string name, Expression<Func<IReadOnlyList<TInput>, int, decimal, IAnalyzeContext<TInput>, decimal?>> expr, bool @override = false)
			=> Register<TInput>(name, expr.Body.ToString(), @override);

		public static bool Register<TInput>(string name, Expression<Func<IReadOnlyList<TInput>, int, decimal, decimal, IAnalyzeContext<TInput>, decimal?>> expr, bool @override = false)
			=> Register<TInput>(name, expr.Body.ToString(), @override);

		public static bool Register<TInput>(string name, Expression<Func<IReadOnlyList<TInput>, int, decimal, decimal, decimal, IAnalyzeContext<TInput>, decimal?>> expr, bool @override = false)
			=> Register<TInput>(name, expr.Body.ToString(), @override);

		public static bool Register(string name, Expression<Func<IReadOnlyList<Candle>, int, IAnalyzeContext<Candle>, decimal?>> expr, bool @override = false)
			=> Register<Candle>(name, expr, @override);

		public static bool Register(string name, Expression<Func<IReadOnlyList<Candle>, int, decimal, IAnalyzeContext<Candle>, decimal?>> expr, bool @override = false)
	        => Register<Candle>(name, expr, @override);

		public static bool Register(string name, Expression<Func<IReadOnlyList<Candle>, int, decimal, decimal, IAnalyzeContext<Candle>, decimal?>> expr, bool @override = false)
			=> Register<Candle>(name, expr, @override);

		public static bool Register(string name, Expression<Func<IReadOnlyList<Candle>, int, decimal, decimal, decimal, IAnalyzeContext<Candle>, decimal?>> expr, bool @override = false)
	        => Register<Candle>(name, expr, @override);

		public static bool Register(string name, string expression, bool @override = false)
            => Register<Candle>(name, expression, @override);

        public static bool Register(string name, IFuncAnalyzable analyzable, bool @override = false)
        {
            if (@override)
                _funcDict.TryRemove(name, out _);
            return _funcDict.TryAdd(name, analyzable.Func);
		}

        public static bool Unregister(string name) => _funcDict.TryRemove(name, out _);

        internal static object Get(string name) => _funcDict[name];
	}
}
