using System.Collections.Generic;
using Trady.Core.Period;

namespace Trady.Core
{
    public interface ITimeSeries<TTick>: IEnumerable<TTick> where TTick: ITick 
    {
        TTick this[int index] { get; }

        void Add(TTick tick);

        void Clear();

        int Count { get; }

        PeriodOption Period { get; }
    }
}
