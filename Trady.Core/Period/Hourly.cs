using System;

namespace Trady.Core.Period
{
    public class Hourly : IntradayPeriodBase
    {
        public override uint NumberOfSecond => 60 * 60;

        public override bool IsTimestamp(DateTime dateTime)
            => dateTime.Minute == 0 && dateTime.Second == 0 && dateTime.Millisecond == 0;
    }
}