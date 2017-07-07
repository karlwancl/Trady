using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Trady.Core;
using Trady.Core.Period;
using YahooFinanceApi;

namespace Trady.Importer
{
    public class YahooFinanceImporter : IImporter
    {
        static readonly DateTime UnixMinDateTime = new DateTime(1901, 12, 13);
        static readonly DateTime UnixMaxDateTime = new DateTime(2038, 1, 19);

        static readonly IDictionary<PeriodOption, Period> PeriodMap = new Dictionary<PeriodOption, Period>
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
        public async Task<IEnumerable<Core.Candle>> ImportAsync(string symbol, DateTime? startTime = default(DateTime?), DateTime? endTime = default(DateTime?), PeriodOption period = PeriodOption.Daily, CancellationToken token = default(CancellationToken))
        {
            if (period != PeriodOption.Daily && period != PeriodOption.Weekly && period != PeriodOption.Monthly)
                throw new ArgumentException("This importer only supports daily, weekly & monthly data");

            var corrStartTime = (startTime < UnixMinDateTime ? UnixMinDateTime : startTime) ?? UnixMinDateTime;
            var corrEndTime = (endTime > UnixMaxDateTime ? UnixMaxDateTime : endTime) ?? UnixMaxDateTime;
            var candles = await Yahoo.GetHistoricalAsync(symbol, corrStartTime, corrEndTime, PeriodMap[period], false, token);

            return candles.Select(c => new Core.Candle(c.DateTime, c.Open, c.High, c.Low, c.Close, c.Volume)).OrderBy(c => c.DateTime);
        }
    }
}