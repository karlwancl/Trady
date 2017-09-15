using System;
using System.Collections.Generic;
using Trady.Core;

namespace Trady.Analysis.Backtest
{
    public class Builder
    {
        private IDictionary<IEnumerable<Candle>, int> _weightings;
        private Predicate<IndexedCandle> _buyRule, _sellRule;

        public Builder() : this(null, null, null)
        {
        }

        private Builder(
            IDictionary<IEnumerable<Candle>, int> weightings, 
            Predicate<IndexedCandle> buyRule, 
            Predicate<IndexedCandle> sellRule)
        {
            _weightings = weightings ?? new Dictionary<IEnumerable<Candle>, int>();
            _buyRule = buyRule;
            _sellRule = sellRule;
        }

        public Builder Add(IEnumerable<Candle> candles, int weighting = 1)
        {
            _weightings.Add(candles, weighting);
            return new Builder(_weightings, _buyRule, _sellRule);
        }

        public Builder Buy(Predicate<IndexedCandle> rule)
            => new Builder(_weightings, ic => _buyRule == null ? rule(ic) : rule(ic) || _buyRule(ic), _sellRule);

        public Builder Sell(Predicate<IndexedCandle> rule)
            => new Builder(_weightings, _buyRule, ic => _sellRule  == null ? rule(ic) : rule(ic) || _sellRule(ic));

        public Runner Build()
            => new Runner(_weightings, _buyRule, _sellRule);
    }
}