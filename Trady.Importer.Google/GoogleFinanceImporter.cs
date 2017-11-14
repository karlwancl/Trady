using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Nuba.Finance.Google;

using Trady.Core.Infrastructure;
using Trady.Core.Period;

namespace Trady.Importer.Google
{
    public class GoogleFinanceImporter : IImporter
    {
        private LatestQuotesService _lqs;

        public GoogleFinanceImporter()
        {
            _lqs = new LatestQuotesService();
        }

        private const char SymbolSeparator = '/';

        private static readonly IDictionary<PeriodOption, int> PeriodMap = new Dictionary<PeriodOption, int>
        {
            {PeriodOption.PerSecond, Frequency.EverySecond},
            {PeriodOption.PerMinute, Frequency.EveryMinute},
            {PeriodOption.Hourly, Frequency.EveryHour},
            {PeriodOption.Daily, Frequency.EveryDay}
        };

        public async Task<IReadOnlyList<IOhlcv>> ImportAsync(string symbol, DateTime? startTime = default(DateTime?), DateTime? endTime = default(DateTime?), PeriodOption period = PeriodOption.Daily, CancellationToken token = default(CancellationToken))
        {
            if (!PeriodMap.TryGetValue(period, out int frequency))
                throw new ArgumentException("This importer only supports second, minute, hourly & daily data");

            if (!symbol.Contains(SymbolSeparator.ToString()))
                throw new ArgumentException("The input symbol should be in the form of \'{Market}/{Symbol}\'");

            string[] syms = symbol.Split(SymbolSeparator);
            var candles = await Task.Run(() => _lqs.GetValues(syms[0].ToUpper(), syms[1], PeriodMap[period], startTime, endTime));
            return candles.Select(c => new Core.Candle(c.Date, c.Open, c.High, c.Low, c.Close, c.Volume)).OrderBy(c => c.DateTime).ToList();
        }
    }
}