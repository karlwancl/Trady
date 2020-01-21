using System;

namespace Trady.Core.Period
{
    public class Per12Hour : IntradayPeriodBase
    {
        public override uint NumberOfSecond => 12 * 60 * 60;

        public override bool IsTimestamp(DateTimeOffset dateTime)
            => dateTime.Hour % 12 == 0 && dateTime.Minute == 0 && dateTime.Second == 0 && dateTime.Millisecond == 0;
    }
}
