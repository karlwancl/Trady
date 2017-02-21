using System;

namespace Trady.Core.Period
{
    public class BiHourly : IntradayPeriodBase
    {
        public override uint NumberOfSecond => 2 * 60 * 60;

        public override bool IsTimestamp(DateTime dateTime)
            => dateTime.Hour % 2 == 0 && dateTime.Minute == 0 && dateTime.Second == 0 && dateTime.Millisecond == 0;
    }
}