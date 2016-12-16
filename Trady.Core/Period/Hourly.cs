using System;
using Trady.Core.Helper;

namespace Trady.Core.Period
{
    public class Hourly : IPeriod, IIntradayPeriod
    {
        public uint NumPerSecond => 60 * 60;

        public bool IsTimestamp(DateTime dateTime)
            => dateTime.Minute == 0 && dateTime.Second == 0 && dateTime.Millisecond == 0;

        public DateTime NextTimestamp(DateTime dateTime)
            => dateTime.Truncate(TimeSpan.FromHours(1)).AddHours(1);
    }
}
