using System;
using Trady.Core.Helper;

namespace Trady.Core.Period
{
    public class Daily : PeriodBase, IInterdayPeriod
    {
        public uint OrderOfTransformation => 1;

        public override bool IsTimestamp(DateTime dateTime)
            => dateTime.TimeOfDay == new TimeSpan(0, 0, 0);

        protected override DateTime ComputeTimestampByCorrectedPeriodCount(DateTime dateTime, int correctedPeriodCount)
            => dateTime.Truncate(TimeSpan.FromDays(1)).AddDays(correctedPeriodCount);
    }
}
