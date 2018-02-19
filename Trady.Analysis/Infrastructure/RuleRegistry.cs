using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using Trady.Analysis.Extension;
using Trady.Analysis.Indicator;
using Trady.Core;
using Trady.Core.Infrastructure;

namespace Trady.Analysis.Infrastructure
{
    public static class RuleRegistry
    {
        private static readonly ScriptOptions options = ScriptOptions.Default
                                                    .WithReferences(typeof(AnalyzeContext).GetTypeInfo().Assembly)
                                                    .WithImports(typeof(AnalyzeContext).Namespace, typeof(SimpleMovingAverage).Namespace, typeof(PredicateExtension).Namespace);

        private static readonly ConcurrentDictionary<string, object> _ruleDict = new ConcurrentDictionary<string, object>();

        public class RuleGlobals
        {
            public IIndexedOhlcv ic;
            public IReadOnlyList<decimal> p;
        }

        private static Func<IIndexedOhlcv, IReadOnlyList<decimal>, bool> CreateRule(string expression)
        {
            var @delegate = CSharpScript.Create<bool>(expression, options, typeof(RuleGlobals)).CreateDelegate();
            return (_ic, _p) => @delegate(new RuleGlobals { ic = _ic, p = _p }).Result;
        }

        private static bool _Register(string name, object obj, bool @override = false)
        {
			if (@override)
            {
                _ruleDict.TryRemove(name, out _);
            }

            return _ruleDict.TryAdd(name, obj);
        }

        public static bool Register(string name, string expression, bool @override = false)
            => _Register(name, CreateRule(expression), @override);

        public static bool Register(string name, Expression<Func<IIndexedOhlcv, IReadOnlyList<decimal> ,bool>> expr, bool @override = false)
            => _Register(name, expr.Compile(), @override);

        public static bool Unregister(string name) => _ruleDict.TryRemove(name, out _);

        internal static object Get(string name) => _ruleDict[name];
    }
}
