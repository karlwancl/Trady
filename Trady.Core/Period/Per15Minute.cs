using System;

namespace Trady.Core.Period
{
    public class Per15Minute : IntradayPeriodBase
    {
        public override uint NumberOfSecond => 15 * 60;

        public override bool IsTimestamp(DateTime dateTime)
            => dateTime.Minute % 15 == 0 && dateTime.Second == 0 && dateTime.Millisecond == 0;
    }
}