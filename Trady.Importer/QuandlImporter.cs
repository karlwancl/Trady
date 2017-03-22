using Quandl.NET;
using Quandl.NET.Model.Enum;
using System;
using System.Collections.Generic;
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
        private static IDictionary<PeriodOption, Collapse> _periodMap = new Dictionary<PeriodOption, Collapse>
        {
            {PeriodOption.Daily, Collapse.Daily },
            {PeriodOption.Weekly, Collapse.Weekly },
            {PeriodOption.Monthly, Collapse.Monthly }
        };

        private QuandlClient _client;
        private string _databaseCode;

        public QuandlImporter(string apiKey, string databaseCode)
        {
            _client = new QuandlClient(apiKey);
            _databaseCode = databaseCode;
        }

        public async Task<Equity> ImportAsync(string symbol, DateTime? startTime = null, DateTime? endTime = null, PeriodOption period = PeriodOption.Daily, CancellationToken token = default(CancellationToken))
        {
            if (period != PeriodOption.Daily && period != PeriodOption.Weekly && period != PeriodOption.Monthly)
                throw new ArgumentException("This importer only supports daily, weekly & monthly data");

            var response = await _client.Dataset.GetAsync(_databaseCode, symbol, startDate: startTime, endDate: endTime, token: token, collapse: _periodMap[period]).ConfigureAwait(false);
            var candles = response.DatasetData.Data.Where(r => !r.IsNullOrWhitespace()).Select(r => r.CreateCandle()).ToList();
            return candles.ToEquity(symbol, period);
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