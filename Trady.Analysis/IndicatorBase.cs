using System.Collections.Generic;
using System.Threading.Tasks;
using Trady.Core;

namespace Trady.Analysis
{
    public abstract class IndicatorBase<TTick> : AnalyzableBase<TTick>, IIndicator where TTick : ITick
    {
        private IIndicatorResultProvider _provider;
        private List<IIndicator> _dependencies;

        public IndicatorBase(Equity equity, params int[] parameters) : base(equity)
        {
            Parameters = parameters;
            _dependencies = new List<IIndicator>();
        }

        protected void RegisterDependencies(params IIndicator[] indicators)
        {
            _dependencies.AddRange(indicators);
        }

        public sealed override TTick ComputeByIndex(int index)
        {
            bool isProviderValid = _provider != null && _provider.IsEquityExists && _provider.IsIndicatorExists;
            bool isIndexValid = index >= 0 && index <= Equity.Count - 1;
            if (isProviderValid && isIndexValid)
            {
                var (hasValue, tick) = _provider.GetAsync<TTick>(Equity[index].DateTime).Result;
                if (hasValue)
                    return tick;
            }
            return ComputeByIndexImpl(index);;
        }

        public async Task InitWithIndicatorResultProviderAsync(IIndicatorResultProvider provider)
        {
            _provider = provider;
            foreach (var dependency in _dependencies)
                await dependency.InitWithIndicatorResultProviderAsync(_provider.Clone());
            await _provider.InitWithIndicatorAsync(this);
        }

        public int[] Parameters { get; private set; }
    }
}