using System;
using System.Collections.Generic;
using System.Text;

namespace Trady.Core.Period
{
    /// <summary>
    /// Use for time-series transformation, applied to period narrower than daily
    /// </summary>
    interface IIntradayPeriod
    {
        /// <summary>
        /// Interval of the period, per second
        /// </summary>
        uint NumberOfSecond { get; }
    }
}
