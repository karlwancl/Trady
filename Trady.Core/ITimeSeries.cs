using System.Collections.Generic;
using Trady.Core.Period;

namespace Trady.Core
{
    public interface ITimeSeries
    {
        IList<ITick> Ticks { get; }
    }

    public interface ITimeSeries<TTick> : IList<TTick>, ITimeSeries where TTick : ITick
    {
        string Name { get; }

        PeriodOption Period { get; }

        int MaxCount { get; }
    }
}