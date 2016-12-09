using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Trady.Analysis;
using Trady.Core;

namespace Trady.Exporter
{
    public abstract class ExporterBase : IExporter
    {
        public abstract Task<bool> ExportAsync(Equity candleTimeSeries, IList<IAnalyticResultTimeSeries> resultTimeSeriesList, DateTime? startTime = default(DateTime?), DateTime? endTime = default(DateTime?), bool ascending = false, CancellationToken token = default(CancellationToken));

        public Task<bool> ExportAsync(Equity candleTimeSeries, IAnalyticResultTimeSeries resultTimeSeries = null, DateTime? startTime = default(DateTime?), DateTime? endTime = default(DateTime?), bool ascending = false, CancellationToken token = default(CancellationToken)) 
            => ExportAsync(candleTimeSeries, new List<IAnalyticResultTimeSeries> { resultTimeSeries }, startTime, endTime, ascending, token);
    }
}
