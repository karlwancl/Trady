using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Trady.Analysis;
using Trady.Core;

namespace Trady.Exporter
{
    public interface IExporter
    {
        Task<bool> ExportAsync(Equity candleTimeSeries, IAnalyticResultTimeSeries resultTimeSeries = null, DateTime? startTime = null, DateTime? endTime = null, bool ascending = false, CancellationToken token = default(CancellationToken));

        Task<bool> ExportAsync(Equity candleTimeSeries, IList<IAnalyticResultTimeSeries> resultTimeSeriesList, DateTime? startTime = null, DateTime? endTime = null, bool ascending = false, CancellationToken token = default(CancellationToken));
    }
}
