using System;
using Trady.Core.Helper;

namespace Trady.Core.Period
{
    public class BiHourly : IPeriod, IIntradayPeriod
    {
        public uint NumPerSecond => 2 * 60 * 60;

        public bool IsTimestamp(DateTime dateTime)
            => dateTime.Hour % 2 == 0 && dateTime.Minute == 0 && dateTime.Second == 0 && dateTime.Millisecond == 0;

        public DateTime NextTimestamp(DateTime dateTime)
            => dateTime.Truncate(TimeSpan.FromHours(2)).AddHours(2);
    }
}
