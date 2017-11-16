using System;

namespace Trady.Core.Period
{
    public abstract class IntradayPeriodBase : PeriodBase, IIntradayPeriod
    {
        public abstract uint NumberOfSecond { get; }

        protected override DateTimeOffset ComputeTimestampByCorrectedPeriodCount(DateTimeOffset dateTime, int correctedPeriodCount)
            => dateTime.DateTime.Truncate(TimeSpan.FromSeconds(NumberOfSecond)).AddSeconds(correctedPeriodCount * NumberOfSecond);
    }
}