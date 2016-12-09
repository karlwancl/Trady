using System;
using System.Collections.Generic;
using System.Linq;
using Trady.Core.Period;

namespace Trady.Core.Helper
{
    public static class TimeSeriesExtension
    {
        public static IPeriod CreateInstance(this PeriodOption period)
        {
            string periodName = Enum.GetName(typeof(PeriodOption), period);
            var periodType = Type.GetType($"{typeof(TimeSeriesBase<>).Namespace}.Period.{periodName}");
            return (IPeriod)Activator.CreateInstance(periodType);
        }

        public static int? FindFirstIndexOrDefault<T>(this IEnumerable<T> items, Predicate<T> predicate)
        {
            for (int i = 0; i < items.Count(); i++)
                if (predicate(items.ElementAt(i)))
                    return i;
            return null;
        }

        public static int? FindLastIndexOrDefault<T>(this IEnumerable<T> items, Predicate<T> predicate)
        {
            for (int i = items.Count() - 1; i >= 0; i--)
                if (predicate(items.ElementAt(i)))
                    return i;
            return null;
        }
    }
}
