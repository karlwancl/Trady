using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Trady.Core;
using Trady.Strategy.Rule;

namespace Trady.Strategy
{
    public class PortfolioBuilder
    {
        private IDictionary<Equity, int> _equityPairs;
        private IList<IRule<ComputableCandle>> _buyRules;
        private IList<IRule<ComputableCandle>> _sellRules;

        public PortfolioBuilder()
        {
            _equityPairs = new Dictionary<Equity, int>();
            _buyRules = new List<IRule<ComputableCandle>>();
            _sellRules = new List<IRule<ComputableCandle>>();
        }

        public PortfolioBuilder Add(Equity equity, int portion = 1)
        {
            if (_equityPairs.TryGetValue(equity, out int equityPortion))
                _equityPairs[equity] = equityPortion + portion;
            else
                _equityPairs.Add(equity, portion);
            return this;
        }

        public PortfolioBuilder Buy(IRule<ComputableCandle> rule)
        {
            _buyRules.Add(rule);
            return this;
        }

        public PortfolioBuilder Sell(IRule<ComputableCandle> rule)
        {
            _sellRules.Add(rule);
            return this;
        }

        public Portfolio Build()
        {
            var buyRule = _buyRules.Aggregate((r0, r) => r0.Or(r));
            var sellRule = _sellRules.Aggregate((r0, r) => r0.Or(r));
            return new Portfolio(_equityPairs, buyRule, sellRule);
        }
    }
}
