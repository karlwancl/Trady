using System.Collections;
using System.Collections.Generic;

namespace Trady.Core.Infrastructure
{
    public interface IIndexedObject
    {
        int Index { get; }

        IEnumerable BackingList { get; }

        IIndexedObject Prev { get; }

        IIndexedObject Next { get; }

        object Underlying { get; }

        IAnalyzeContext Context { get; set; }
    }

    public interface IIndexedObject<T> : IIndexedObject
    {
        new IEnumerable<T> BackingList { get; }

        new IIndexedObject<T> Prev { get; }

        new IIndexedObject<T> Next { get; }

        new T Underlying { get; }

        new IAnalyzeContext<T> Context { get; set; }
    }
}