using System;
using Trady.Core.Helper;

namespace Trady.Core.Period
{
    public class PerSecond : IPeriod, IIntradayPeriod
    {
        public uint NumPerSecond => 1;

        public bool IsTimestamp(DateTime dateTime)
            => dateTime.Millisecond == 0;

        public DateTime NextTimestamp(DateTime dateTime)
            => dateTime.Truncate(TimeSpan.FromSeconds(1)).AddSeconds(1);
    }
}
