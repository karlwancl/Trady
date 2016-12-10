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

        public override async Task<bool> ExportAsync(Equity equity, IList<IAnalyticResultTimeSeries> resultTimeSeriesList, DateTime? startTime = default(DateTime?), DateTime? endTime = default(DateTime?), bool ascending = false, CancellationToken token = default(CancellationToken))
        {
            return await Task.Factory.StartNew(() =>
            {
                if (equity == null)
                    throw new ArgumentNullException(nameof(equity));

                using (var fs = File.OpenWrite(_path))
                using (var sw = new StreamWriter(fs))
                using (var csvWriter = new CsvWriter(sw))
                {
                    WriteHeader(resultTimeSeriesList, csvWriter);
                    WriteRecords(equity, resultTimeSeriesList, startTime, endTime, ascending, csvWriter);
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
                    if (resultTimeSeries != null && resultTimeSeries.Any())
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

        private static void WriteRecords(Equity equity, IList<IAnalyticResultTimeSeries> resultTimeSeriesList, DateTime? startTime, DateTime? endTime, bool ascending, CsvWriter csvWriter)
        {
            for (int i = 0; i < equity.Count; i++)
            {
                var candle = equity[ascending ? i : equity.Count - i - 1];

                var currentDateTime = candle.DateTime;
                if (startTime.HasValue && currentDateTime < startTime.Value || endTime.HasValue && currentDateTime >= endTime.Value)
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
                        if (resultTimeSeries != null)
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
                }

                csvWriter.NextRecord();
            }
        }
    }
}
