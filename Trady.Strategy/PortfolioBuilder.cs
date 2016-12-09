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
        private IList<Equity> _equities;
        private IList<IRule<Equity>> _buyRules;
        private IList<IRule<Equity>> _sellRules;

        public PortfolioBuilder()
        {
            _equities = new List<Equity>();
            _buyRules = new List<IRule<Equity>>();
            _sellRules = new List<IRule<Equity>>();
        }

        public PortfolioBuilder AddEquity(Equity equity)
        {
            _equities.Add(equity);
            return this;
        }

        public PortfolioBuilder BuyWhen(IRule<Equity> rule)
        {
            _buyRules.Add(rule);
            return this;
        }

        public PortfolioBuilder SellWhen(IRule<Equity> rule)
        {
            _sellRules.Add(rule);
            return this;
        }

        public Portfolio Build()
        {
            var buyRule = _buyRules.Aggregate((r0, r) => r0.Or(r));
            var sellRule = _sellRules.Aggregate((r0, r) => r0.Or(r));
            return new Portfolio(_equities, buyRule, sellRule);
        }
    }
}
