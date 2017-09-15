using System;
using System.Collections;
using System.Collections.Generic;

namespace Trady.Core.Infrastructure
{
    public interface IAnalyzeContext : IDisposable
    {
        TAnalyzable Get<TAnalyzable>(params object[] parameters) where TAnalyzable : IAnalyzable;

        IEnumerable BackingList { get; }
    }

    public interface IAnalyzeContext<TInput> : IAnalyzeContext
    {
        new IEnumerable<TInput> BackingList { get; }
    }
}
