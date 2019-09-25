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

        IIndexedObject Before(int count);

        IIndexedObject After(int count);

        IIndexedObject First { get; }

        IIndexedObject Last { get; }
    }

    public interface IIndexedObject<T> : IIndexedObject
    {
        new IEnumerable<T> BackingList { get; }

        new IIndexedObject<T> Prev { get; }

        new IIndexedObject<T> Next { get; }

        new T Underlying { get; }

        new IAnalyzeContext<T> Context { get; set; }

        new IIndexedObject<T> Before(int count);

        new IIndexedObject<T> After(int count);

        new IIndexedObject<T> First { get; }

        new IIndexedObject<T> Last { get; }
    }
}
