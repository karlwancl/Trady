using System;
using Trady.Core.Helper;

namespace Trady.Core.Period
{
    public class Daily : IPeriod, IInterdayPeriod
    {
        public uint NumPerDay => 1;

        public bool IsTimestamp(DateTime dateTime)
            => dateTime.TimeOfDay == new TimeSpan(0, 0, 0);

        public DateTime NextTimestamp(DateTime dateTime)
            => dateTime.Truncate(TimeSpan.FromDays(1)).AddDays(1);
    }
}
