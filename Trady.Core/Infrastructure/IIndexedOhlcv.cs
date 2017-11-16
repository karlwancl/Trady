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

        TAnalyzable Get<TAnalyzable>(params object[] @params)
            where TAnalyzable : IAnalyzable;

        IFuncAnalyzable<IAnalyzableTick<decimal?>> GetFunc(string name, params decimal[] @params);

        bool Eval(string name, params decimal[] @params);
    }
}