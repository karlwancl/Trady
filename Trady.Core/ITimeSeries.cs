using System.Collections;
using System.Collections.Generic;
using Trady.Core.Period;

namespace Trady.Core
{
    public interface ITimeSeries : IEnumerable
    {
        string Name { get; }

        void Reset();

        int TickCount { get; }

        PeriodOption Period { get; }

        ITick this[int index] { get; }

        void Add(ITick tick);
    }

    public interface ITimeSeries<TTick>: IEnumerable<TTick>, ITimeSeries where TTick: ITick 
    {
        new TTick this[int index] { get; }

        void Add(TTick tick);
    }
}
