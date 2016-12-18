using System;
using Trady.Core.Helper;

namespace Trady.Core.Period
{
    public class Per30Minute : IntradayPeriodBase
    {
        public override uint NumberOfSecond => 30 * 60;

        public override bool IsTimestamp(DateTime dateTime)
            => dateTime.Minute % 30 == 0 && dateTime.Second == 0 && dateTime.Millisecond == 0;
    }
}
