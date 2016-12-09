using System;
using System.Threading;
using System.Threading.Tasks;
using Trady.Core;
using Trady.Core.Period;

namespace Trady.Importer
{
    public interface IImporter
    {
        Task<Equity> ImportAsync(string symbol, DateTime? startTime = null, DateTime? endTime = null, PeriodOption period = PeriodOption.Daily, CancellationToken token = default(CancellationToken));
    }
}
