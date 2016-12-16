using CsvHelper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Trady.Core;
using Trady.Core.Period;
using Trady.Importer.Helper;
using Trady.Logging;

namespace Trady.Importer
{
    public class CsvImporter : IImporter
    {
        private string _path;
        private static ILogger _logger = new LoggerFactory().CreateLogger<CsvImporter>();

        public CsvImporter(string path)
        {
            _path = path;
        }

        public async Task<Equity> ImportAsync(string symbol, DateTime? startTime = null, DateTime? endTime = null, PeriodOption period = PeriodOption.Daily, CancellationToken token = default(CancellationToken))
        {
            return await Task.Factory.StartNew(() =>
            {
                using (var fs = File.OpenRead(_path))
                using (var sr = new StreamReader(fs))
                using (var csvReader = new CsvReader(sr))
                {
                    var candles = new List<Candle>();
                    while (csvReader.Read())
                    {
                        var record = csvReader.CurrentRecord;

                        try
                        {
                            var recordDatetime = Convert.ToDateTime(record[0]);
                            if (startTime.HasValue && recordDatetime < startTime.Value || endTime.HasValue && recordDatetime >= endTime.Value)
                                continue;

                            candles.Add(record.CreateCandleFromRow());
                        }
                        catch (Exception ex)
                        {
                            Logger.Warn($"The record {record[0]} will be skipped. ex: {ex.ToString()}");
                        }
                    }
                    return new Equity(symbol, candles).Transform(period);
                }
            });
        }
    }
}
