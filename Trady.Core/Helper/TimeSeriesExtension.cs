using System;
using System.Collections.Generic;
using Trady.Core.Period;

namespace Trady.Core.Helper
{
    public static class TimeSeriesExtension
    {
        public static IPeriod CreateInstance(this PeriodOption period, Country? country = null)
        {
            string periodName = Enum.GetName(typeof(PeriodOption), period);
            var periodType = Type.GetType($"{typeof(TimeSeries<>).Namespace}.Period.{periodName}");
            if (!country.HasValue || periodType is IIntradayPeriod)
                return (IPeriod)Activator.CreateInstance(periodType);
            else
                return (IPeriod)Activator.CreateInstance(periodType, new object[] { country.Value });
        }

        public static int? FindIndexOrDefault<T>(this List<T> items, Predicate<T> predicate)
        {
            int index = items.FindIndex(predicate);
            return index == -1 ? (int?)null : index;
        }

        public static int? FindLastIndexOrDefault<T>(this List<T> items, Predicate<T> predicate)
        {
            int index = items.FindLastIndex(predicate);
            return index == -1 ? (int?)null : index;
        }

        public static Equity ToEquity(this IList<Candle> candles, string name, PeriodOption period = PeriodOption.Daily, int maxTickCount = 65536)
            => new Equity(name, candles, period, maxTickCount);
    }
}