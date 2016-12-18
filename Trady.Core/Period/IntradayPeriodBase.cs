using System;
using Trady.Core.Helper;

namespace Trady.Core.Period
{
    public abstract class IntradayPeriodBase : PeriodBase, IIntradayPeriod
    {
        public abstract uint NumberOfSecond { get; }

        protected override DateTime ComputeTimestampByCorrectedPeriodCount(DateTime dateTime, int correctedPeriodCount)
            => dateTime.Truncate(TimeSpan.FromSeconds(NumberOfSecond)).AddSeconds(correctedPeriodCount * NumberOfSecond);
    }
}
