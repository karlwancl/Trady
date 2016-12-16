using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Trady.Analysis;
using Trady.Analysis.Indicator;
using Trady.Core;

namespace Trady.Exporter
{
    public interface IExporter
    {
        Task<bool> ExportAsync(Equity equity, ITimeSeries resultTimeSeries = null, DateTime? startTime = null, DateTime? endTime = null, bool ascending = false, CancellationToken token = default(CancellationToken));

        Task<bool> ExportAsync(Equity equity, IList<ITimeSeries> resultTimeSeriesList, DateTime? startTime = null, DateTime? endTime = null, bool ascending = false, CancellationToken token = default(CancellationToken));
    }
}
