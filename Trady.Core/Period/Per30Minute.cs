using System;
using Trady.Core.Helper;

namespace Trady.Core.Period
{
    public class Per30Minute : IPeriod, IIntradayPeriod
    {
        public uint NumPerSecond => 30 * 60;

        public bool IsTimestamp(DateTime dateTime)
            => dateTime.Minute % 30 == 0 && dateTime.Second == 0 && dateTime.Millisecond == 0;

        public DateTime NextTimestamp(DateTime dateTime)
            => dateTime.Truncate(TimeSpan.FromMinutes(30)).AddMinutes(30);
    }
}
