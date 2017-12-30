using System;
namespace Trady.Core.Period
{
    public class Per10Minute : IntradayPeriodBase
    {
        public override uint NumberOfSecond => 10 * 60;

        public override bool IsTimestamp(DateTimeOffset dateTime)
            => dateTime.Minute % 10 == 0 && dateTime.Second == 0 && dateTime.Millisecond == 0;
    }
}
