using System;
namespace Trady.Analysis.Infrastructure
{
    public interface IAnalyzableTick
    {
		DateTime? DateTime { get; }
	}

    public interface IAnalyzableTick<T>: IAnalyzableTick
    {
        T Tick { get; }
    }
}
