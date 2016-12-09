using System;

namespace Trady.Core.Period
{
    public class Weekly : IPeriod, IInterdayPeriod
    {
        public uint NumPerDay => 7;

        public bool IsTimestamp(DateTime dateTime)
            => dateTime.DayOfWeek == DayOfWeek.Sunday && dateTime.TimeOfDay == new TimeSpan(0, 0, 0);

        public DateTime NextTimestamp(DateTime dateTime)
        {
            int day = (int)dateTime.DayOfWeek;
            const int numDayInWeek = 7;
            return dateTime.AddDays(numDayInWeek - day).Date;
        }
    }
}
