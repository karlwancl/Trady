using Quandl.NET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Trady.Core;
using Trady.Core.Period;
using Trady.Importer.Helper;

namespace Trady.Importer
{
    public abstract class QuandlImporterBase : IImporter
    {
        private QuandlClient _client;

        protected QuandlImporterBase(string apiKey)
        {
            _client = new QuandlClient(apiKey);
        }

        protected abstract string DatabaseCode { get; }

        public async Task<Equity> ImportAsync(string symbol, DateTime? startTime = null, DateTime? endTime = null, PeriodOption period = PeriodOption.Daily, CancellationToken token = default(CancellationToken))
        {
            var response = await _client.Dataset.GetAsync(DatabaseCode, symbol, startDate: startTime, endDate: endTime, token: token).ConfigureAwait(false);
            var candles = response.DatasetData.Data.Where(r => !r.IsRowNullOrWhiteSpace()).Select(r => r.CreateCandleFromRow());
            return new Equity(symbol, candles.ToList()).Transform(period);
        }
    }
}
