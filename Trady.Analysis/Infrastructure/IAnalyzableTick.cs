using System;
namespace Trady.Analysis.Infrastructure
{
    public interface IAnalyzableTick
    {
		DateTime? DateTime { get; }

        object Tick { get; }
	}

    public interface IAnalyzableTick<T>: IAnalyzableTick
    {
        new T Tick { get; }
    }
}
