using System;
using System.Threading.Tasks;
using Trady.Core;

namespace Trady.Analysis
{
    public interface IDataProvider
    {
        bool IsEquityExists { get; }

        bool IsIndicatorExists { get; }

        IDataProvider Clone();

        Task InitWithIndicatorAsync(IIndicator indicator);

        Task<(bool HasValue, TTick Value)> GetAsync<TTick>(DateTime dateTime) where TTick : ITick;
    }
}
