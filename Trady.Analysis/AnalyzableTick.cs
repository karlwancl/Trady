using System;
using Trady.Core.Infrastructure;

namespace Trady.Analysis
{
    public class AnalyzableTick<T> : IAnalyzableTick<T>
    {
        public DateTime? DateTime { get; }
        public T Tick { get; }

        object IAnalyzableTick.Tick => Tick;

        public AnalyzableTick(DateTime? dateTime, T tick)
        {
            Tick = tick;
            DateTime = dateTime;
        }
    }
}