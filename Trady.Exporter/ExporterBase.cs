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
        public abstract Task<bool> ExportAsync(Equity equity, IList<ITimeSeries> resultTimeSeriesList, DateTime? startTime = default(DateTime?), DateTime? endTime = default(DateTime?), bool ascending = false, CancellationToken token = default(CancellationToken));

        public Task<bool> ExportAsync(Equity equity, ITimeSeries resultTimeSeries = null, DateTime? startTime = default(DateTime?), DateTime? endTime = default(DateTime?), bool ascending = false, CancellationToken token = default(CancellationToken)) 
            => ExportAsync(equity, new List<ITimeSeries> { resultTimeSeries }, startTime, endTime, ascending, token);
    }
}
