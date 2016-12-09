using CsvHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Trady.Analysis;
using Trady.Core;

namespace Trady.Exporter
{
    public class CsvExporter : ExporterBase
    {
        private string _path;

        public CsvExporter(string path)
        {
            _path = path;
        }

        public override async Task<bool> ExportAsync(Equity candleTimeSeries, IList<IAnalyticResultTimeSeries> resultTimeSeriesList, DateTime? startTime = default(DateTime?), DateTime? endTime = default(DateTime?), bool ascending = false, CancellationToken token = default(CancellationToken))
        {
            return await Task.Factory.StartNew(() =>
            {
                if (candleTimeSeries == null)
                    throw new ArgumentNullException(nameof(candleTimeSeries));

                using (var fs = File.OpenWrite(_path))
                using (var sw = new StreamWriter(fs))
                using (var csvWriter = new CsvWriter(sw))
                {
                    WriteHeader(resultTimeSeriesList, csvWriter);
                    WriteRecords(candleTimeSeries, resultTimeSeriesList, startTime, endTime, ascending, csvWriter);
                }

                return true;
            }, token);
        }
        private static void WriteHeader(IList<IAnalyticResultTimeSeries> resultTimeSeriesList, CsvWriter csvWriter)
        {
            csvWriter.WriteField("DateTime");
            csvWriter.WriteField("Open");
            csvWriter.WriteField("High");
            csvWriter.WriteField("Low");
            csvWriter.WriteField("Close");
            csvWriter.WriteField("Volume");
            if (resultTimeSeriesList != null)
            {
                foreach (var resultTimeSeries in resultTimeSeriesList)
                {
                    if (resultTimeSeries.Any())
                    {
                        foreach (var valueKvp in resultTimeSeries.First().Values)
                        {
                            csvWriter.WriteField(valueKvp.Key);
                        }
                    }
                }
            }
            csvWriter.NextRecord();
        }

        private static void WriteRecords(Equity candleTimeSeries, IList<IAnalyticResultTimeSeries> resultTimeSeriesList, DateTime? startTime, DateTime? endTime, bool ascending, CsvWriter csvWriter)
        {
            for (int i = 0; i < candleTimeSeries.Count; i++)
            {
                var candle = candleTimeSeries[ascending ? i : candleTimeSeries.Count - i - 1];

                var currentDateTime = candle.DateTime;
                if (currentDateTime < startTime || currentDateTime >= endTime)
                    continue;

                csvWriter.WriteField(candle.DateTime);
                csvWriter.WriteField(candle.Open);
                csvWriter.WriteField(candle.High);
                csvWriter.WriteField(candle.Low);
                csvWriter.WriteField(candle.Close);
                csvWriter.WriteField(candle.Volume);

                if (resultTimeSeriesList != null)
                {
                    foreach (var resultTimeSeries in resultTimeSeriesList)
                    {
                        var currentResult = resultTimeSeries.FirstOrDefault(r => r.DateTime == currentDateTime);
                        if (currentResult == null)
                            throw new KeyNotFoundException("You should ensure that the indicator result time series is computed from the input candle time series!");
                        foreach (var valueKvp in currentResult.Values)
                        {
                            csvWriter.WriteField(valueKvp.Value);
                        }
                    }
                }

                csvWriter.NextRecord();
            }
        }
    }
}
