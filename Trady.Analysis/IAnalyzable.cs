using System;
using System.Threading.Tasks;
using Trady.Analysis.Provider;
using Trady.Core;

namespace Trady.Analysis
{
    public interface IAnalyzable
    {
        Equity Equity { get; }

        Task InitWithTickProviderAsync(ITickProvider provider);
    }
}