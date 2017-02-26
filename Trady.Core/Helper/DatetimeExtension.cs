using System;
using Trady.Core.Period;

namespace Trady.Core.Helper
{
    public static class DateTimeExtension
    {
        internal static DateTime Truncate(this DateTime dateTime, TimeSpan timeSpan)
        {
            if (timeSpan == TimeSpan.Zero) return dateTime; // Or could throw an ArgumentException
            return dateTime.AddTicks(-(dateTime.Ticks % timeSpan.Ticks));
        }

        public static IPeriod CreateInstance(this PeriodOption period, Country? country = null)
        {
            string periodName = Enum.GetName(typeof(PeriodOption), period);
            var periodType = Type.GetType($"Trady.Core.Period.{periodName}");
            if (!country.HasValue || periodType is IIntradayPeriod)
                return (IPeriod)Activator.CreateInstance(periodType);
            else
                return (IPeriod)Activator.CreateInstance(periodType, new object[] { country.Value });
        }
    }
}