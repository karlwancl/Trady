using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using CsvHelper;
using CsvHelper.Configuration;

using Trady.Core;
using Trady.Core.Infrastructure;
using Trady.Core.Period;

namespace Trady.Importer.Csv
{
    public class CsvImporter : IImporter
    {
        private string _path;
        private readonly CultureInfo _culture;
        private string _format;
        private string _delimiter;
        private bool _hasHeader = true;

        public CsvImporter(string path) : this(path, CultureInfo.CurrentCulture)
        {            
        }

        public CsvImporter(string path, CultureInfo culture)
        {
            _path = path;
            _culture = culture;
        }

        public CsvImporter(string path, CsvImportConfiguration configuration): this(path, configuration.CultureInfo)
        {
            _format = configuration.DateFormat;
            _delimiter = configuration.Delimiter;
            _hasHeader = configuration.HasHeaderRecord;
        }

        public async Task<IReadOnlyList<IOhlcv>> ImportAsync(string symbol, DateTime? startTime = null, DateTime? endTime = null, PeriodOption period = PeriodOption.Daily, CancellationToken token = default(CancellationToken))
            => await Task.Factory.StartNew(() =>
            {
                using (var fs = File.OpenRead(_path))
                using (var sr = new StreamReader(fs))
                using (var csvReader = new CsvReader(sr, new Configuration() { CultureInfo = _culture, Delimiter = string.IsNullOrWhiteSpace(_delimiter) ? "," : _delimiter, HasHeaderRecord = _hasHeader }))
                {
                    var candles = new List<IOhlcv>();
                    bool isHeaderBypassed = false;
                    while (csvReader.Read())
                    {
                        // HasHeaderRecord is not working for CsvReader 6.0.2
                        if (_hasHeader && !isHeaderBypassed)
                        {
                            isHeaderBypassed = true;
                            continue;
                        }

                        var date = string.IsNullOrWhiteSpace(_format) ? csvReader.GetField<DateTime>(0) : DateTime.ParseExact(csvReader.GetField<string>(0), _format, _culture);                        
                        if ((!startTime.HasValue || date >= startTime) && (!endTime.HasValue || date <= endTime))
                            candles.Add(GetRecord(csvReader));
                    }
                    return candles.OrderBy(c => c.DateTime).ToList();
                }
            });

        public IOhlcv GetRecord(CsvReader csv)
        {
            // By using GetField Methodo of the CSV Reader Culture Info set in the configuration is used
            return new Candle(
                string.IsNullOrWhiteSpace(_format) ? csv.GetField<DateTime>(0) : DateTime.ParseExact(csv.GetField<string>(0), _format, _culture),
                csv.GetField<Decimal>(1),
                csv.GetField<Decimal>(2),
                csv.GetField<Decimal>(3),
                csv.GetField<Decimal>(4),
                csv.GetField<Decimal>(5)
            );
        }
    }
}
