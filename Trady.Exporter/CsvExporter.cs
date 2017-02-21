using CsvHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Trady.Core;
using Trady.Exporter.Helper;

namespace Trady.Exporter
{
    public class CsvExporter : ExporterBase
    {
        private string _path;

        public CsvExporter(string path)
        {
            _path = path;
        }

        public override async Task<bool> ExportAsync(Equity equity, IList<ITimeSeries> resultTimeSeriesList, DateTime? startTime = default(DateTime?), DateTime? endTime = default(DateTime?), bool ascending = false, CancellationToken token = default(CancellationToken))
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

        private static void WriteHeader(IList<ITimeSeries> resultTimeSeriesList, CsvWriter csvWriter)
        {
            new List<string> { "DateTime", "Open", "High", "Low", "Close", "Volume" }
                .ForEach(n => csvWriter.WriteField(n));

            resultTimeSeriesList?
                .Where(ts => ts != null)
                .ToList()
                .ForEach(ts => ts.GetGenericParameterPropertiesNames().ToList().ForEach(n => csvWriter.WriteField(n)));

            csvWriter.NextRecord();
        }

        private static void WriteRecords(Equity equity, IList<ITimeSeries> resultTimeSeriesList, DateTime? startTime, DateTime? endTime, bool ascending, CsvWriter csvWriter)
        {
            var maxTickCountAmongTs = resultTimeSeriesList?.Max(ts => ts.Ticks.Count) ?? equity.Count;
            var tsWithMaxTickCount = resultTimeSeriesList?.First(ts => ts.Ticks.Count == maxTickCountAmongTs) ?? equity;

            for (int i = 0; i < maxTickCountAmongTs; i++)
            {
                var currentDateTime = tsWithMaxTickCount.Ticks[i].DateTime;
                if (startTime.HasValue && currentDateTime < startTime.Value || endTime.HasValue && currentDateTime >= endTime.Value)
                    continue;

                var candle = equity.FirstOrDefault(c => c.DateTime == currentDateTime);

                new List<object> { currentDateTime, candle?.Open, candle?.High, candle?.Low, candle?.Close, candle?.Volume }
                    .ForEach(o => csvWriter.WriteField(o ?? ""));

                resultTimeSeriesList?
                    .Where(ts => ts != null && ts.Ticks != null && ts.Ticks.Any())
                    .ToList()
                    .ForEach(ts =>
                    {
                        var result = ts.Ticks.FirstOrDefault(r => r.DateTime == currentDateTime);
                        if (result != null)
                            result
                            .GetType()
                            .GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)
                            .Select(p => p.GetValue(result, null))
                            .ToList()
                            .ForEach(o => csvWriter.WriteField(o ?? ""));
                        else
                        {
                            ts.Ticks
                            .First()
                            .GetType()
                            .GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)
                            .ToList()
                            .ForEach(p => csvWriter.WriteField(""));
                        }
                    });

                csvWriter.NextRecord();
            }
        }
    }
}