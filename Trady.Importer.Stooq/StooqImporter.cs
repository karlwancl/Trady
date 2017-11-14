using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using StooqApi;

using Trady.Core.Infrastructure;
using Trady.Core.Period;

namespace Trady.Importer.Stooq
{
    public class StooqImporter : IImporter
    {
        private static readonly IDictionary<PeriodOption, Period> PeriodMap = new Dictionary<PeriodOption, Period>
        {
            {PeriodOption.Daily, Period.Daily},
            {PeriodOption.Monthly, Period.Monthly},
            {PeriodOption.Weekly, Period.Weekly}
        };

        /// <summary>
        /// Imports the async. endTime stock history inclusive
        /// </summary>
        /// <returns>The async.</returns>
        /// <param name="symbol">Symbol.</param>
        /// <param name="startTime">Start time.</param>
        /// <param name="endTime">End time.</param>
        /// <param name="period">Period.</param>
        /// <param name="token">Token.</param>
        public async Task<IReadOnlyList<IOhlcv>> ImportAsync(string symbol, DateTime? startTime = default(DateTime?), DateTime? endTime = default(DateTime?), PeriodOption period = PeriodOption.Daily, CancellationToken token = default(CancellationToken))
        {
            if (period != PeriodOption.Daily && period != PeriodOption.Monthly && period != PeriodOption.Weekly)
                throw new ArgumentException("This importer only supports daily, weekly & monthly data");

            var candles = await StooqApi.Stooq.GetHistoricalAsync(symbol, PeriodMap[period], startTime, endTime, SkipOption.None, false, token);
            return candles.Select(c => new Core.Candle(c.DateTime, c.Open, c.High, c.Low, c.Close, c.Volume)).OrderBy(c => c.DateTime).ToList();
        }
    }
}