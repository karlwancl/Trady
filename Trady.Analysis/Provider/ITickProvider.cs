using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Trady.Core;

namespace Trady.Analysis.Provider
{
    public interface ITickProvider
    {
        bool IsReady { get; }

        ITickProvider Clone();

        Task InitWithAnalyzableAsync(IAnalyzable analyzable);

        Task<IEnumerable<TTick>> GetAllAsync<TTick>() where TTick : ITick;
    }
}
