using System;

namespace Trady.Core.Period
{
    public class Daily : InterdayPeriodBase
    {
        public Daily() : base()
        {
        }

        public override uint OrderOfTransformation => 1;

        public override bool IsTimestamp(DateTimeOffset dateTime)
            => dateTime.TimeOfDay == new TimeSpan(0, 0, 0);

        protected override DateTimeOffset ComputeTimestampByCorrectedPeriodCount(DateTimeOffset dateTime, int correctedPeriodCount)
            => dateTime.DateTime.Truncate(TimeSpan.FromDays(1)).AddDays(correctedPeriodCount);

        protected override DateTimeOffset FloorByDay(DateTimeOffset dateTime, bool isPositivePeriodCount)
            => dateTime.AddDays(isPositivePeriodCount ? 1 : -1);
    }
}