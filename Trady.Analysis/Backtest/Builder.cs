using System;
using System.Collections.Generic;
using Trady.Core;
using Trady.Core.Infrastructure;

namespace Trady.Analysis.Backtest
{
    public class Builder
    {
        private IDictionary<IEnumerable<IOhlcvData>, int> _weightings;
        private Predicate<IIndexedOhlcvData> _buyRule, _sellRule;

        public Builder() : this(null, null, null)
        {
        }

        private Builder(
            IDictionary<IEnumerable<IOhlcvData>, int> weightings, 
            Predicate<IIndexedOhlcvData> buyRule, 
            Predicate<IIndexedOhlcvData> sellRule)
        {
            _weightings = weightings ?? new Dictionary<IEnumerable<IOhlcvData>, int>();
            _buyRule = buyRule;
            _sellRule = sellRule;
        }

        public Builder Add(IEnumerable<IOhlcvData> candles, int weighting = 1)
        {
            _weightings.Add(candles, weighting);
            return new Builder(_weightings, _buyRule, _sellRule);
        }

        public Builder Buy(Predicate<IIndexedOhlcvData> rule)
            => new Builder(_weightings, ic => _buyRule == null ? rule(ic) : rule(ic) || _buyRule(ic), _sellRule);

        public Builder Sell(Predicate<IIndexedOhlcvData> rule)
            => new Builder(_weightings, _buyRule, ic => _sellRule  == null ? rule(ic) : rule(ic) || _sellRule(ic));

        public Runner Build()
            => new Runner(_weightings, _buyRule, _sellRule);
    }
}