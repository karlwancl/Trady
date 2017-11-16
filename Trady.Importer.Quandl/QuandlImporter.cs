using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Quandl.NET;

using Trady.Core.Infrastructure;
using Trady.Core.Period;

namespace Trady.Importer.Quandl
{
    public class QuandlImporter : IImporter
    {
        private static readonly IDictionary<PeriodOption, Collapse> PeriodMap = new Dictionary<PeriodOption, Collapse>
        {
            {PeriodOption.Daily, Collapse.Daily },
            {PeriodOption.Weekly, Collapse.Weekly },
            {PeriodOption.Monthly, Collapse.Monthly }
        };

        private readonly QuandlClient _client;
        private readonly string _databaseCode;

        public QuandlImporter(string apiKey, string databaseCode)
        {
            _client = new QuandlClient(apiKey);
            _databaseCode = databaseCode;
        }

        public async Task<IReadOnlyList<IOhlcv>> ImportAsync(string symbol, DateTime? startTime = null, DateTime? endTime = null, PeriodOption period = PeriodOption.Daily, CancellationToken token = default(CancellationToken))
        {
            if (period != PeriodOption.Daily && period != PeriodOption.Weekly && period != PeriodOption.Monthly)
                throw new ArgumentException("This importer only supports daily, weekly & monthly data");

            var response = await _client.Timeseries.GetDataAsync(_databaseCode, symbol, startDate: startTime, endDate: endTime, token: token, collapse: PeriodMap[period]).ConfigureAwait(false);
            return response.DatasetData.Data.Where(r => !r.IsNullOrWhitespace()).Select(r => r.CreateIOhlcvData()).OrderBy(c => c.DateTime).ToList();
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