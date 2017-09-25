using System;
using System.Collections;
using System.Collections.Generic;

namespace Trady.Core.Infrastructure
{
    public interface IAnalyzeContext : IDisposable
    {
        TAnalyzable Get<TAnalyzable>(params object[] parameters) where TAnalyzable : IAnalyzable;

        IFuncAnalyzable GetFunc(string name, params decimal[] parameters);

		IEnumerable BackingList { get; }
    }

    public interface IAnalyzeContext<TInput> : IAnalyzeContext
    {
        // Either IFuncAnalyzable<decimal?> or IFuncAnalyzable<AnalyzableTick<decimal?>>
		new IFuncAnalyzable<dynamic> GetFunc(string name, params decimal[] parameters);

        Predicate<T> GetRule<T>(string name, params decimal[] parameters) where T : IIndexedObject<TInput>;

		new IEnumerable<TInput> BackingList { get; }
	}
}
