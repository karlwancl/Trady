using Quandl.NET;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Trady.Core;
using Trady.Core.Helper;
using Trady.Core.Period;
using Trady.Importer.Helper;

namespace Trady.Importer
{
    public class QuandlImporter : IImporter
    {
        private QuandlClient _client;
        private string _databaseCode;

        public QuandlImporter(string apiKey, string databaseCode)
        {
            _client = new QuandlClient(apiKey);
            _databaseCode = databaseCode;
        }

        public async Task<Equity> ImportAsync(string symbol, DateTime? startTime = null, DateTime? endTime = null, PeriodOption period = PeriodOption.Daily, CancellationToken token = default(CancellationToken))
        {
            var response = await _client.Dataset.GetAsync(_databaseCode, symbol, startDate: startTime, endDate: endTime, token: token).ConfigureAwait(false);
            var candles = response.DatasetData.Data.Where(r => !r.IsRowNullOrWhiteSpace()).Select(r => r.CreateCandleFromRow()).ToList();
            return candles.ToEquity(symbol).Transform(period);
        }
    }

    public class QuandlWikiImporter : QuandlImporter
    {
        public QuandlWikiImporter(string apiKey) : base(apiKey, "WIKI")
        {
        }
    }

    public class QuandlYahooImporter : QuandlImporter
    {
        public QuandlYahooImporter(string apiKey) : base(apiKey, "YAHOO")
        {
        }
    }

    public class QuandlChrisImporter : QuandlImporter
    {
        public QuandlChrisImporter(string apiKey) : base(apiKey, "CHRIS")
        {
        }
    }
}