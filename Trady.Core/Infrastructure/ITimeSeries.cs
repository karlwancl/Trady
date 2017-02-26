using System.Collections.Generic;
using Trady.Core.Period;

namespace Trady.Core.Infrastructure
{
    public interface ITimeSeries<TTick> : ICollection<TTick> where TTick : ITick
    {
        /// <summary>
        /// Name of the time series
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Period of the time series
        /// </summary>
        PeriodOption Period { get; }
    }
}