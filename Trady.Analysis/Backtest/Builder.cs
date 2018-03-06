using System;
using System.Collections.Generic;
using Trady.Core.Infrastructure;

namespace Trady.Analysis.Backtest
{
    public class Builder
    {
        private readonly IDictionary<IEnumerable<IOhlcv>, int> _weightings;
        private readonly Predicate<IIndexedOhlcv> _buyRule;
        private readonly Predicate<IIndexedOhlcv> _sellRule;
        private readonly decimal _fee;
        private readonly bool _buyCompleteBaseCurrencies;

        public Builder() : this(null, null, null, 0, true)
        {
        }

        private Builder(
            IDictionary<IEnumerable<IOhlcv>, int> weightings, 
            Predicate<IIndexedOhlcv> buyRule, 
            Predicate<IIndexedOhlcv> sellRule,
            decimal fee,
            bool buyCompleteBaseCurrencies)
        {
            _weightings = weightings ?? new Dictionary<IEnumerable<IOhlcv>, int>();
            _buyRule = buyRule;
            _sellRule = sellRule;
            _fee = fee;
            _buyCompleteBaseCurrencies = buyCompleteBaseCurrencies;
        }

        public Builder Add(IEnumerable<IOhlcv> candles, int weighting = 1)
        {
            _weightings.Add(candles, weighting);
            return new Builder(_weightings, _buyRule, _sellRule, _fee, _buyCompleteBaseCurrencies);
        }

        public Builder Fee(decimal fee)
        {
            return new Builder(_weightings, _buyRule, _sellRule, fee, _buyCompleteBaseCurrencies);
        }

        public Builder BuyPartialCurrencies()
        {
            return new Builder(_weightings, _buyRule, _sellRule, _fee, false);
        }
        
        public Builder Buy(Predicate<IIndexedOhlcv> rule)
            => new Builder(_weightings, ic => _buyRule == null ? rule(ic) : rule(ic) || _buyRule(ic), _sellRule, _fee, _buyCompleteBaseCurrencies);

        public Builder Sell(Predicate<IIndexedOhlcv> rule)
            => new Builder(_weightings, _buyRule, ic => _sellRule  == null ? rule(ic) : rule(ic) || _sellRule(ic), _fee, _buyCompleteBaseCurrencies);

        public Runner Build()
            => new Runner(_weightings, _buyRule, _sellRule, _fee, _buyCompleteBaseCurrencies);
    }
}
