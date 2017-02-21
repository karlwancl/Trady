using EnricoApi;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Trady.Core.Period
{
    public abstract class InterdayPeriodBase : PeriodBase, IInterdayPeriod
    {
        private IMemoryCache _cache;

        private MemoryCacheEntryOptions _policy = new MemoryCacheEntryOptions
        {
            SlidingExpiration = TimeSpan.FromDays(1)
        };

        private static Country GetCurrentCountry()
        {
            var countryName = RegionInfo.CurrentRegion.EnglishName.Replace(" ", "");
            if (!Enum.TryParse(countryName, out Country country))
                country = Country.UnitedStatesOfAmerica;
            return country;
        }

        protected InterdayPeriodBase() : this(GetCurrentCountry())
        {
        }

        protected InterdayPeriodBase(Country country)
        {
            Country = country;
            _cache = new MemoryCache(new MemoryCacheOptions());
        }

        public Country Country { get; private set; }

        public abstract uint OrderOfTransformation { get; }

        public async Task<DateTime> BusinessTimestampAtAsync(DateTime dateTime, int periodCount)
        {
            if (periodCount == 0)
                throw new ArgumentException("Timestamp at 0 is undefined, you should use non-zero periodCount");

            var correctedPeriodCount = periodCount + ((periodCount < 0 && !IsTimestamp(dateTime)) ? 1 : 0);
            var dateTimeCursor = dateTime;
            for (int i = 1; i <= Math.Abs(periodCount); i++)
            {
                dateTimeCursor = ComputeTimestampByCorrectedPeriodCount(dateTimeCursor, Math.Sign(periodCount));
                while (!await IsBusinessDayAsync(dateTimeCursor).ConfigureAwait(false))
                    dateTimeCursor = FloorByDay(dateTimeCursor, Math.Sign(periodCount) > 0);
            }
            return dateTimeCursor;
        }

        protected abstract DateTime FloorByDay(DateTime dateTime, bool isPositivePeriodCount);

        public async Task<DateTime> NextBusinessTimestampAsync(DateTime dateTime) => await BusinessTimestampAtAsync(dateTime, 1).ConfigureAwait(false);

        public async Task<DateTime> PrevBusinessTimestampAsync(DateTime dateTime) => await BusinessTimestampAtAsync(dateTime, -1).ConfigureAwait(false);

        protected async Task<bool> IsBusinessDayAsync(DateTime dateTime)
        {
            if (dateTime.DayOfWeek == DayOfWeek.Saturday || dateTime.DayOfWeek == DayOfWeek.Sunday)
                return false;

            var countryName = Enum.GetName(typeof(Country), Country);
            if (!Enum.TryParse(countryName, out EnricoApi.Country enricoCountry))
                enricoCountry = EnricoApi.Country.UnitedStatesOfAmerica;

            if (!_cache.TryGetValue(dateTime.Year, out IList<Holiday> holidays))
            {
                holidays = await Enrico.GetPublicHolidaysForYearAsync(dateTime.Year, enricoCountry).ConfigureAwait(false);
                _cache.Set(dateTime.Year, holidays, _policy);
            }

            return !holidays.Any(h => h.DateTime.Date.Equals(dateTime.Date));
        }
    }
}