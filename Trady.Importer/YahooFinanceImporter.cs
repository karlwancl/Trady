using System;
using System.Collections.Generic;
using System.Linq;
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
        private static IDictionary<PeriodOption, Period> PeriodMap = new Dictionary<PeriodOption, Period>
        {
            {PeriodOption.Daily, Period.Daily },
            {PeriodOption.Weekly, Period.Weekly },
            {PeriodOption.Monthly, Period.Monthly }
        };

        public async Task<Equity> ImportAsync(string symbol, DateTime? startTime = default(DateTime?), DateTime? endTime = default(DateTime?), PeriodOption period = PeriodOption.Daily, CancellationToken token = default(CancellationToken))
        {
            var yPeriod = Period.Daily;
            PeriodMap.TryGetValue(period, out yPeriod);

            var yCandles = await Yahoo.GetHistoricalAsync(symbol, startTime, endTime, yPeriod, false, token);

            var output = new List<Core.Candle>();
            foreach (var yCandle in yCandles)
                output.Add(new Core.Candle(yCandle.DateTime, yCandle.Open, yCandle.High, yCandle.Low, yCandle.Close, yCandle.Volume));

            PeriodMap.ToDictionary(kvp => kvp.Value, kvp => kvp.Key).TryGetValue(yPeriod, out PeriodOption ryPeriod);
            return output.ToEquity(symbol, ryPeriod);
        }
    }
}