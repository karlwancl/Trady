using System;
using Trady.Core.Helper;

namespace Trady.Core.Period
{
    public class PerMinute : IPeriod, IIntradayPeriod
    {
        public uint NumPerSecond => 60;

        public bool IsTimestamp(DateTime dateTime)
            => dateTime.Millisecond == 0 && dateTime.Second == 0;

        public DateTime NextTimestamp(DateTime dateTime)
            => dateTime.Truncate(TimeSpan.FromMinutes(1)).AddMinutes(1);
    }
}
