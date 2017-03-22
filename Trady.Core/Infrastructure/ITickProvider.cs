using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Trady.Core.Infrastructure
{
    public interface ITickProvider
    {
        bool IsReady { get; }

        ITickProvider Clone();

        Task InitWithAnalyzableAsync(IAnalyzable analyzable);

        Task<IEnumerable<TTick>> GetAsync<TTick>(DateTime startTime) where TTick : ITick;
    }
}