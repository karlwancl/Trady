using System;
using System.Collections.Generic;
using System.Text;

namespace Trady.Core.Period
{
    public class Monthly : PeriodBase, IInterdayPeriod
    {
        public uint OrderOfTransformation => 30;

        public override bool IsTimestamp(DateTime dateTime)
            => dateTime.Day == 1 && dateTime.TimeOfDay == new TimeSpan(0, 0, 0);

        protected override DateTime ComputeTimestampByCorrectedPeriodCount(DateTime dateTime, int correctedPeriodCount)
            => new DateTime(dateTime.Year, dateTime.Month, 1).AddMonths(correctedPeriodCount);
    }
}
