using EnricoApi;
using System;
using System.Threading.Tasks;

namespace Trady.Core.Period
{
    public class Weekly : InterdayPeriodBase
    {
        public Weekly() : base()
        {

        }

        public Weekly(Country country) : base(country)
        {
        }

        public override uint OrderOfTransformation => 7;

        public override bool IsTimestamp(DateTime dateTime)
            => dateTime.DayOfWeek == DayOfWeek.Sunday && dateTime.TimeOfDay == new TimeSpan(0, 0, 0);

        protected override DateTime ComputeTimestampByCorrectedPeriodCount(DateTime dateTime, int correctedPeriodCount)
            => dateTime.AddDays(-(int) dateTime.DayOfWeek).AddDays(correctedPeriodCount* 7).Date;

        protected override DateTime FloorByDay(DateTime dateTime, bool isPositivePeriodCount)
            => dateTime.AddDays(1);
    }
}
