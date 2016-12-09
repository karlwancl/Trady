using System;
using Trady.Core.Helper;

namespace Trady.Core.Period
{
    public class Per15Minute : IPeriod, IIntradayPeriod
    {
        public uint NumPerSecond => 15 * 60;

        public bool IsTimestamp(DateTime dateTime)
            => dateTime.Minute % 15 == 0 && dateTime.Second == 0 && dateTime.Millisecond == 0;

        public DateTime NextTimestamp(DateTime dateTime)
            => dateTime.Truncate(TimeSpan.FromMinutes(15)).AddMinutes(15);
    }
}
