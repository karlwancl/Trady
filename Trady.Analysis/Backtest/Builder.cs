using System;
using System.Collections.Generic;
using Trady.Analysis.Backtest.FeeCalculators;
using Trady.Core.Infrastructure;

namespace Trady.Analysis.Backtest
{
    public class Builder
    {
        private readonly IDictionary<IEnumerable<IOhlcv>, int> _weightings;
        private readonly Predicate<IIndexedOhlcv> _buyRule;
        private readonly Predicate<IIndexedOhlcv> _sellRule;
        private readonly bool _buyInCompleteQuantity;
        private readonly IFeeCalculator _calculator;

        public Builder() : this(null, null, null, true, null)
        {
        }

        private Builder(
            IDictionary<IEnumerable<IOhlcv>, int> weightings, 
            Predicate<IIndexedOhlcv> buyRule, 
            Predicate<IIndexedOhlcv> sellRule,
            bool buyInCompleteQuantity,
            IFeeCalculator calculator)
        {
            _weightings = weightings ?? new Dictionary<IEnumerable<IOhlcv>, int>();
            _buyRule = buyRule;
            _sellRule = sellRule;
            _buyInCompleteQuantity = buyInCompleteQuantity;

            //Use default calculator if none defined.
            _calculator = calculator ?? new FeeCalculator();
        }

        public Builder Add(IEnumerable<IOhlcv> candles, int weighting = 1)
        {
            _weightings.Add(candles, weighting);
            return new Builder(_weightings, _buyRule, _sellRule, _buyInCompleteQuantity, _calculator);
        }

        public Builder FlatExchangeFeeRate(decimal flatExchangeFeeRate)
        {
            var calculator = new FeeCalculator(flatExchangeFeeRate, 1);
            return new Builder(_weightings, _buyRule, _sellRule, _buyInCompleteQuantity, calculator);
        }

        public Builder Calculator(IFeeCalculator calculator)
            => new Builder(_weightings, _buyRule, _sellRule, _buyInCompleteQuantity, calculator);
        

        public Builder BuyWithAllAvailableCash()
            => new Builder(_weightings, _buyRule, _sellRule, false, _calculator);
        
        public Builder Buy(Predicate<IIndexedOhlcv> rule)
            => new Builder(_weightings, ic => _buyRule == null ? rule(ic) : rule(ic) || _buyRule(ic), _sellRule, _buyInCompleteQuantity, _calculator);

        public Builder Sell(Predicate<IIndexedOhlcv> rule)
            => new Builder(_weightings, _buyRule, ic => _sellRule  == null ? rule(ic) : rule(ic) || _sellRule(ic), _buyInCompleteQuantity, _calculator);

        /// <summary>
        /// The cost per trade/transaction.
        /// </summary>
        /// <param name="premium">A numerical value that represents the cost of each transaction.</param>
        /// <returns></returns>
        public Builder Premium(decimal premium)
        {
            var calculator = new FeeCalculator(0, premium);           
            return new Builder(_weightings, _buyRule, _sellRule, _buyInCompleteQuantity, calculator);
        }

        public Builder PremiumAndFees(decimal premium, decimal flatExchangeFeeRate)
        {
            var calculator = new FeeCalculator(flatExchangeFeeRate, premium);
           
            return new Builder(_weightings, _buyRule, _sellRule, _buyInCompleteQuantity, calculator);
        }


        public Runner Build()
            => new Runner(_weightings, _buyRule, _sellRule, _buyInCompleteQuantity, _calculator);
            
    }
}
