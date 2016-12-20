using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Trady.Core.Period
{
    /// <summary>
    /// Use for time-series transformation, applied to period wider than or equals daily
    /// </summary>
    public interface IInterdayPeriod
    {
        /// <summary>
        /// Any number representing order of transformation, recommend using number of days
        /// </summary>
        uint OrderOfTransformation { get; }

        Country Country { get; }

        Task<DateTime> PrevBusinessTimestampAsync(DateTime dateTime);

        Task<DateTime> NextBusinessTimestampAsync(DateTime dateTime);

        Task<DateTime> BusinessTimestampAtAsync(DateTime dateTime, int periodCount);
    }
}
