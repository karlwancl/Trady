using System;
using Trady.Core.Infrastructure;

namespace Trady.Analysis
{
    public class AnalyzableTick<T> : IAnalyzableTick<T>
    {
        public DateTimeOffset? DateTime { get; }

        public T Tick { get; }

        object IAnalyzableTick.Tick => Tick;

        DateTimeOffset ITick.DateTime => DateTime.GetValueOrDefault();

        public AnalyzableTick(DateTimeOffset? dateTime, T tick)
        {
            Tick = tick;
            DateTime = dateTime;
        }
    }
}