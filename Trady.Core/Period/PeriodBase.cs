using System;

namespace Trady.Core.Period
{
    public abstract class PeriodBase : IPeriod
    {
        public abstract bool IsTimestamp(DateTimeOffset dateTime);

        public DateTimeOffset NextTimestamp(DateTimeOffset dateTime) => TimestampAt(dateTime, 1);

        public DateTimeOffset PrevTimestamp(DateTimeOffset dateTime) => TimestampAt(dateTime, -1);

        public DateTimeOffset TimestampAt(DateTimeOffset dateTime, int periodCount)
        {
            if (periodCount == 0)
                throw new ArgumentException("Timestamp at 0 is undefined, you should use non-zero periodCount");

            // periodCount-1 if periodCount is negative & not a timestamp, since there is truncation in internal implementation.
            var correctedPeriodCount = periodCount + ((periodCount < 0 && !IsTimestamp(dateTime)) ? 1 : 0);
            return ComputeTimestampByCorrectedPeriodCount(dateTime, correctedPeriodCount);
        }

        protected abstract DateTimeOffset ComputeTimestampByCorrectedPeriodCount(DateTimeOffset dateTime, int correctedPeriodCount);
    }
}