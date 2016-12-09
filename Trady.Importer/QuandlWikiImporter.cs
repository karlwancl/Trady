using Quandl.NET;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Trady.Core;
using Trady.Core.Period;
using Trady.Importer.Helper;

namespace Trady.Importer
{
    public class QuandlWikiImporter : IImporter
    {
        private const string DatabaseCode = "WIKI";

        private QuandlClient _client;

        public QuandlWikiImporter(string apiKey)
        {
            _client = new QuandlClient(apiKey);
        }

        public async Task<Equity> ImportAsync(string symbol, DateTime? startTime = null, DateTime? endTime = null, PeriodOption period = PeriodOption.Daily, CancellationToken token = default(CancellationToken))
        {
            var response = await _client.Dataset.GetAsync(DatabaseCode, symbol, startDate: startTime, endDate: endTime, token: token).ConfigureAwait(false);
            var candles = response.DatasetData.Data.Where(r => !r.IsRowNullOrWhiteSpace()).Select(r => CreateCandleFromRow(r));
            return new Equity(symbol, candles.ToList()).Transform(period);
        }

        private static Candle CreateCandleFromRow(object[] row)
        {
            return new Candle(
                Convert.ToDateTime(row[0]),
                Convert.ToDecimal(row[1]),
                Convert.ToDecimal(row[2]),
                Convert.ToDecimal(row[3]),
                Convert.ToDecimal(row[4]),
                Convert.ToInt64(row[5]));
        }
    }
}
