using System;
using System.Threading.Tasks;
using Trady.Core;

namespace Trady.Analysis
{
    public interface IIndicatorResultProvider
    {
        bool IsEquityExists { get; }

        bool IsIndicatorExists { get; }

        IIndicatorResultProvider Clone();

        Task InitWithIndicatorAsync(IIndicator indicator);

        Task<(bool HasValue, TTick Value)> GetAsync<TTick>(DateTime dateTime) where TTick : ITick;
    }
}
