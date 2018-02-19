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
        private static readonly ScriptOptions options = ScriptOptions.Default
                                                    .WithReferences(typeof(AnalyzeContext).GetTypeInfo().Assembly)
                                                    .WithImports(typeof(AnalyzeContext).Namespace, typeof(SimpleMovingAverage).Namespace);

        private static readonly ConcurrentDictionary<string, object> _funcDict = new ConcurrentDictionary<string, object>();

		public class FuncGlobals<TInput>
		{
			public IReadOnlyList<TInput> c;
			public int i;
            public IReadOnlyList<decimal> p;
			public IAnalyzeContext<TInput> ctx;
		}

        private static Func<IReadOnlyList<TInput>, int, IReadOnlyList<decimal>, IAnalyzeContext<TInput>, decimal?> CreateFunc<TInput>(string expression)
		{
			var @delegate = CSharpScript.Create<decimal?>(expression, options, typeof(FuncGlobals<TInput>)).CreateDelegate();
			return (_c, _i, _p, _ctx) => @delegate(new FuncGlobals<TInput> { c = _c, i = _i, p = _p, ctx = _ctx }).Result;
		}

        private static bool _Register(string name, object obj, bool @override = false)
        {
			if (@override)
            {
                _funcDict.TryRemove(name, out _);
            }

            return _funcDict.TryAdd(name, obj);
        }

        public static bool Register<TInput>(string name, string expression, bool @override = false)
            => _Register(name, CreateFunc<TInput>(expression), @override);

        public static bool Register<TInput>(string name, Expression<Func<IReadOnlyList<TInput>, int, IReadOnlyList<decimal>, IAnalyzeContext<TInput>, decimal?>> expr, bool @override = false)
            => _Register(name, expr.Compile(), @override);

		public static bool Register(string name, Expression<Func<IReadOnlyList<IOhlcv>, int, IReadOnlyList<decimal>, IAnalyzeContext<IOhlcv>, decimal?>> expr, bool @override = false)
			=> Register<IOhlcv>(name, expr, @override);

		public static bool Register(string name, string expression, bool @override = false)
            => Register<IOhlcv>(name, expression, @override);

        public static bool Register(string name, IFuncAnalyzable analyzable, bool @override = false)
            => _Register(name, analyzable.Func, @override);

        public static bool Unregister(string name) => _funcDict.TryRemove(name, out _);

        internal static object Get(string name) => _funcDict[name];
	}
}
