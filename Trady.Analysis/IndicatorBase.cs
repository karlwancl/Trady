using System.Collections.Generic;
using System.Threading.Tasks;
using Trady.Core;

namespace Trady.Analysis
{
    public abstract class IndicatorBase<TTick> : AnalyzableBase<TTick>, IIndicator where TTick : ITick
    {
        private IDataProvider _provider;
        private List<IIndicator> _dependents;

        public IndicatorBase(Equity equity, params int[] parameters) : base(equity)
        {
            Parameters = parameters;
            _dependents = new List<IIndicator>();
        }

        protected void RegisterDependents(params IIndicator[] indicators)
        {
            _dependents.AddRange(indicators);
        }

        public sealed override TTick ComputeByIndex(int index)
        {
            bool isProviderValid = _provider != null && _provider.IsEquityExists && _provider.IsIndicatorExists;
            bool isIndexValid = index >= 0 && index <= Equity.Count - 1;
            if (isProviderValid && isIndexValid)
            {
                ///
                var (hasValue, tick) = _provider.GetAsync<TTick>(Equity[index].DateTime).Result;
                if (hasValue)
                    return tick;
            }
            return ComputeByIndexImpl(index);;
        }

        public async Task InitWithDataProviderAsync(IDataProvider provider)
        {
            _provider = provider;
            foreach (var dependent in _dependents)
                await dependent.InitWithDataProviderAsync(_provider.Clone());
            await _provider.InitWithIndicatorAsync(this);
        }

        public int[] Parameters { get; private set; }
    }
}