using System;
using System.Collections.Generic;

namespace Trady.Core.Infrastructure
{
    public interface IIndexedOhlcv : IOhlcv, IIndexedObject<IOhlcv>
    {
        new IEnumerable<IOhlcv> BackingList { get; }

        new IIndexedOhlcv Prev { get; }

        new IIndexedOhlcv Next { get; }

        new IOhlcv Underlying { get; }

        new IAnalyzeContext<IOhlcv> Context { get; set; }

        new IIndexedOhlcv Before(int count);

        new IIndexedOhlcv After(int count);

        [Obsolete]
        TAnalyzable Get<TAnalyzable>(params object[] @params)
            where TAnalyzable : IAnalyzable;

        [Obsolete]
        IFuncAnalyzable<IAnalyzableTick<decimal?>> GetFunc(string name, params decimal[] @params);

        [Obsolete]
        bool Eval(string name, params decimal[] @params);

        decimal? EvalDecimal<TAnalyzable>(params object[] @params)
            where TAnalyzable : IAnalyzable<IAnalyzableTick<decimal?>>;

        bool? EvalBool<TAnalyzable>(params object[] @params)
            where TAnalyzable : IAnalyzable<IAnalyzableTick<bool?>>;

        decimal? EvalFunc(string name, params decimal[] @params);
    }
}
