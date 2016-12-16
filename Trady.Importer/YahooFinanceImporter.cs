using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Trady.Core;
using Trady.Core.Period;
using Flurl;
using Flurl.Http;
using System.IO;
using CsvHelper;
using Trady.Logging;
using Trady.Importer.Helper;

namespace Trady.Importer
{
    public class YahooFinanceImporter : IImporter
    {
        private const string YahooFinanceUrl = "http://ichart.finance.yahoo.com/table.csv";
        private const string SymbolTag = "s";
        private const string FromMonthTag = "a";
        private const string FromDayTag = "b";
        private const string FromYearTag = "c";
        private const string ToMonthTag = "d";
        private const string ToDayTag = "e";
        private const string ToYearTag = "f";
        private const string PeriodTag = "g";
        private const string MonthlyValue = "m";
        private const string DailyValue = "d";
        private const string IgnoreTag = "ignore";
        private const string CsvExtValue = ".csv";

        public async Task<Equity> ImportAsync(string symbol, DateTime? startTime = default(DateTime?), DateTime? endTime = default(DateTime?), PeriodOption period = PeriodOption.Daily, CancellationToken token = default(CancellationToken))
        {
            var dataStream = await YahooFinanceUrl
                .SetQueryParam(SymbolTag, symbol)
                .SetQueryParam(FromMonthTag, startTime?.Month)
                .SetQueryParam(FromDayTag, startTime?.Day)
                .SetQueryParam(FromYearTag, startTime?.Year)
                .SetQueryParam(ToMonthTag, endTime?.Month)
                .SetQueryParam(ToDayTag, endTime?.Day)
                .SetQueryParam(ToYearTag, endTime?.Year)
                .SetQueryParam(PeriodTag, period == PeriodOption.Monthly ? MonthlyValue : (period == PeriodOption.Daily ? DailyValue : throw new ArgumentException("The period type is not supported for this api")))
                .SetQueryParam(IgnoreTag, CsvExtValue)
                .GetAsync(token)
                .ReceiveStream();

            using (var sr = new StreamReader(dataStream))
            using (var csvReader = new CsvReader(sr))
            {
                var candles = new List<Candle>();
                while (csvReader.Read())
                {
                    var record = csvReader.CurrentRecord;
                    try
                    {
                        var recordDatetime = Convert.ToDateTime(record[0]);
                        candles.Add(record.CreateCandleFromRow());
                    }
                    catch (Exception ex)
                    {
                        Logger.Warn($"The record {record[0]} will be skipped. ex: {ex.ToString()}");
                    }
                }
                return new Equity(symbol, candles).Transform(period);
            }
        }
    }
}
