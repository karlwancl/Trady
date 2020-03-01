using System;

namespace Trady.Core.Period
{
    public class Per4Hour : IntradayPeriodBase
    {
        public override uint NumberOfSecond => 4 * 60 * 60;

        public override bool IsTimestamp(DateTimeOffset dateTime)
            => dateTime.Hour % 4 == 0 && dateTime.Minute == 0 && dateTime.Second == 0 && dateTime.Millisecond == 0;
    }
}
