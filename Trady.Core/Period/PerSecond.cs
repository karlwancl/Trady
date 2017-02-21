using System;

namespace Trady.Core.Period
{
    public class PerSecond : IntradayPeriodBase
    {
        public override uint NumberOfSecond => 1;

        public override bool IsTimestamp(DateTime dateTime)
            => dateTime.Millisecond == 0;
    }
}