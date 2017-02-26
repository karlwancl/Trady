using System;
using System.Threading.Tasks;

namespace Trady.Core.Infrastructure
{
    public interface IAnalyzable
    {
        Equity Equity { get; }

        Task InitWithTickProviderAsync(ITickProvider provider);
    }

    public interface IAnalyzable<TTick> : IAnalyzable where TTick : ITick
    {
        TimeSeries<TTick> Compute(DateTime? startTime, DateTime? endTime);

        TTick ComputeByDateTime(DateTime dateTime);

        TTick ComputeByIndex(int index);
    }
}