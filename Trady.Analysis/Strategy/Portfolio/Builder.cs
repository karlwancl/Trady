using System;
using System.Collections.Generic;
using Trady.Analysis.Strategy.Rule;
using Trady.Core;

namespace Trady.Analysis.Strategy.Portfolio
{
    public class Builder
    {
		IDictionary<IEnumerable<Candle>, int> _weightings;
        IRule<IndexedCandle> _buyRule, _sellRule;

		public Builder() : this(null, null, null)
		{
		}

		Builder(IDictionary<IEnumerable<Candle>, int> weightings, IRule<IndexedCandle> buyRule, IRule<IndexedCandle> sellRule)
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

        public Builder Buy(IRule<IndexedCandle> rule)
            => new Builder(_weightings, _buyRule?.Or(rule) ?? rule, _sellRule);

        public Builder Sell(IRule<IndexedCandle> rule)
            => new Builder(_weightings, _buyRule, _sellRule?.Or(rule) ?? rule);

        public Runner Build()
            => new Runner(_weightings, _buyRule, _sellRule);
    }
}
