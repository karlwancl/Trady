using System;
using System.Collections.Generic;
using System.Text;

namespace Trady.Core.Period
{
    /// <summary>
    /// Use for time-series transformation, applied to period wider than or equals daily
    /// </summary>
    interface IInterdayPeriod
    {
        /// <summary>
        /// Any number representing order of transformation, recommend using number of days
        /// </summary>
        uint NumPerDay { get; }
    }
}
