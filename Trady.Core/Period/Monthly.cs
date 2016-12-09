using System;
using System.Collections.Generic;
using System.Text;

namespace Trady.Core.Period
{
    public class Monthly : IPeriod, IInterdayPeriod
    {
        public uint NumPerDay => 30;

        public bool IsTimestamp(DateTime dateTime)
            => dateTime.Day == 1 && dateTime.TimeOfDay == new TimeSpan(0, 0, 0);

        public DateTime NextTimestamp(DateTime dateTime)
        {
            var newDateTime = dateTime.AddMonths(1);
            return new DateTime(newDateTime.Year, newDateTime.Month, 1);
        }
    }
}
