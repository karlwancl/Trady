using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Trady.Core;
using Trady.Core.Helper;
using Trady.Core.Period;
using YahooFinanceApi;

namespace Trady.Importer
{
    public class YahooFinanceImporter : IImporter
    {
        private static IDictionary<PeriodOption, Period> _periodMap = new Dictionary<PeriodOption, Period>
        {
            {PeriodOption.Daily, Period.Daily },
            {PeriodOption.Weekly, Period.Weekly },
            {PeriodOption.Monthly, Period.Monthly }
        };

        public async Task<Equity> ImportAsync(string symbol, DateTime? startTime = default(DateTime?), DateTime? endTime = default(DateTime?), PeriodOption period = PeriodOption.Daily, CancellationToken token = default(CancellationToken))
        {
            if (period != PeriodOption.Daily && period != PeriodOption.Weekly && period != PeriodOption.Monthly)
                throw new ArgumentException("This importer only supports daily, weekly & monthly data");

            var yahooCandles = await Yahoo.GetHistoricalAsync(symbol, startTime, endTime, _periodMap[period], false, token);

            var output = new List<Core.Candle>();
            foreach (var yahooCandle in yahooCandles)
                output.Add(new Core.Candle(yahooCandle.DateTime, yahooCandle.Open, yahooCandle.High, yahooCandle.Low, yahooCandle.Close, yahooCandle.Volume));

            return output.ToEquity(symbol, period);
        }
    }
}