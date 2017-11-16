using System;

namespace Trady.Core.Period
{
    public interface IPeriod
    {
        DateTimeOffset TimestampAt(DateTimeOffset dateTime, int periodCount);

        DateTimeOffset PrevTimestamp(DateTimeOffset dateTime);

        DateTimeOffset NextTimestamp(DateTimeOffset dateTime);

        bool IsTimestamp(DateTimeOffset dateTime);
    }
}