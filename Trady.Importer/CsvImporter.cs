using CsvHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Trady.Core;
using Trady.Core.Period;

namespace Trady.Importer
{
    public class CsvImporter : IImporter
    {
        private string _path;

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

                        var recordSymbol = Convert.ToString(record[0]);
                        if (!recordSymbol.Equals(symbol, StringComparison.OrdinalIgnoreCase))
                            continue;

                        var recordStartTime = Convert.ToDateTime(record[1]);
                        if (recordStartTime < startTime || recordStartTime >= endTime)
                            continue;

                        candles.Add(CreateCandleFromRow(record));
                    }
                    return new Equity(symbol, candles).Transform(period);
                }
            });
        }

        private static Candle CreateCandleFromRow(object[] row)
        {
            return new Candle(
                Convert.ToDateTime(row[1]),
                Convert.ToDecimal(row[2]),
                Convert.ToDecimal(row[3]),
                Convert.ToDecimal(row[4]),
                Convert.ToDecimal(row[5]),
                Convert.ToInt64(row[6]));
        }
    }
}
