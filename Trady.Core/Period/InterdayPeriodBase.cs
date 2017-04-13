using System;

namespace Trady.Core.Period
{
    public abstract class InterdayPeriodBase : PeriodBase, IInterdayPeriod
    {
        protected InterdayPeriodBase() : base()
        {
        }

        public abstract uint OrderOfTransformation { get; }

        protected abstract DateTime FloorByDay(DateTime dateTime, bool isPositivePeriodCount);
    }
}