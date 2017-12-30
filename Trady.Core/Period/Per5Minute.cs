using System;
namespace Trady.Core.Period
{
    public class Per5Minute : IntradayPeriodBase
    {
        public override uint NumberOfSecond => 5 * 60; 

        public override bool IsTimestamp(DateTimeOffset dateTime)
            => dateTime.Minute % 5 == 0 && dateTime.Second == 0 && dateTime.Millisecond == 0;
    }
}
