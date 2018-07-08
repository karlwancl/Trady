using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Trady.Core.Infrastructure;
using Trady.Core.Period;

using YahooFinanceApi;

namespace Trady.Importer.Yahoo
{
    public class YahooFinanceImporter : IImporter
    {
        private static readonly DateTime UnixMinDateTime = new DateTime(1901, 12, 13);
        private static readonly DateTime UnixMaxDateTime = new DateTime(2038, 1, 19);

        private static readonly IDictionary<PeriodOption, Period> PeriodMap = new Dictionary<PeriodOption, Period>
        {
            {PeriodOption.Daily, Period.Daily },
            {PeriodOption.Weekly, Period.Weekly },
            {PeriodOption.Monthly, Period.Monthly }
        };

        /// <summary>
        /// Imports the async. Endtime stock history exclusive
        /// </summary>
        /// <returns>The async.</returns>
        /// <param name="symbol">Symbol.</param>
        /// <param name="startTime">Start time.</param>
        /// <param name="endTime">End time.</param>
        /// <param name="period">Period.</param>
        /// <param name="token">Token.</param>
        public async Task<IReadOnlyList<IOhlcv>> ImportAsync(string symbol, DateTime? startTime = default(DateTime?), DateTime? endTime = default(DateTime?), PeriodOption period = PeriodOption.Daily, CancellationToken token = default(CancellationToken))
        {
            if (period != PeriodOption.Daily && period != PeriodOption.Weekly && period != PeriodOption.Monthly)
                throw new ArgumentException("This importer only supports daily, weekly & monthly data");

            var corrStartTime = (startTime < UnixMinDateTime ? UnixMinDateTime : startTime) ?? UnixMinDateTime;
            var corrEndTime = (endTime > UnixMaxDateTime ? UnixMaxDateTime : endTime) ?? UnixMaxDateTime;
            var candles = await YahooFinanceApi.Yahoo.GetHistoricalAsync(symbol, corrStartTime, corrEndTime, PeriodMap[period], token);

            return candles.Select(c => new Core.Candle(c.DateTime, c.Open, c.High, c.Low, c.Close, c.Volume)).OrderBy(c => c.DateTime).ToList();
        }
    }
}
