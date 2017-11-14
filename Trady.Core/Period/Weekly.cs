using System;

namespace Trady.Core.Period
{
    public class Weekly : InterdayPeriodBase
    {
        public Weekly() : base()
        {
        }

        public override uint OrderOfTransformation => 7;

        public override bool IsTimestamp(DateTimeOffset dateTime)
            => dateTime.DayOfWeek == DayOfWeek.Sunday && dateTime.TimeOfDay == new TimeSpan(0, 0, 0);

        protected override DateTimeOffset ComputeTimestampByCorrectedPeriodCount(DateTimeOffset dateTime, int correctedPeriodCount)
            => dateTime.AddDays(-(int)dateTime.DayOfWeek).AddDays(correctedPeriodCount * 7).Date;

        protected override DateTimeOffset FloorByDay(DateTimeOffset dateTime, bool isPositivePeriodCount)
            => dateTime.AddDays(1);
    }
}