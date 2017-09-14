using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Trady.Analysis.Helper;
using Trady.Analysis.Strategy.Rule;
using Trady.Core;

namespace Trady.Analysis.Strategy.Portfolio
{
    public class Runner
    {
        private IDictionary<IEnumerable<Candle>, int> _weightings;
        private IRule<IndexedCandle> _buyRule, _sellRule;

        internal Runner(IDictionary<IEnumerable<Candle>, int> weightings, IRule<IndexedCandle> buyRule, IRule<IndexedCandle> sellRule)
        {
            _weightings = weightings;
            _buyRule = buyRule;
            _sellRule = sellRule;
        }

        public event BuyHandler OnBought;

        public delegate void BuyHandler(IEnumerable<Candle> candles, int index, DateTime dateTime, decimal buyPrice, int quantity, decimal absCashFlow, decimal currentCashAmount);

        public event SellHandler OnSold;

        public delegate void SellHandler(IEnumerable<Candle> candles, int index, DateTime dateTime, decimal sellPrice, int quantity, decimal absCashFlow, decimal currentCashAmount, decimal plRatio);

        public async Task<Result> RunAsync(decimal principal, decimal premium = 1.0m, DateTime? startTime = null, DateTime? endTime = null)
            => await Task.Factory.StartNew(() => Run(principal, premium, startTime, endTime));

        public Result Run(decimal principal, decimal premium = 1.0m, DateTime? startTime = null, DateTime? endTime = null)
        {
            if (_weightings == null || !_weightings.Any())
                throw new ArgumentException("You should have at least one candle set for calculation");

            // Distribute principal to each candle set
            decimal totalWeight = _weightings.Sum(w => w.Value);
            IReadOnlyDictionary<IEnumerable<Candle>, decimal> preAssetCashMap = _weightings.ToDictionary(w => w.Key, w => principal * w.Value / totalWeight);
            var assetCashMap = preAssetCashMap.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

            // Init transaction history
            var transactions = new List<Transaction>();

            // Loop with each asset
            for (int i = 0; i < _weightings.Count; i++)
            {
                var asset = assetCashMap.ElementAt(i).Key;
                var startIndex = asset.FindIndexOrDefault(c => c.DateTime >= (startTime ?? DateTime.MinValue), 0).Value;
                var endIndex = asset.FindLastIndexOrDefault(c => c.DateTime <= (endTime ?? DateTime.MaxValue), asset.Count() - 1).Value;
                CreateRuleExecutor(asset, premium, assetCashMap, transactions).Execute(asset, startIndex, endIndex);
            }

            return new Result(preAssetCashMap, assetCashMap, transactions);
        }

        private BuySellRuleExecutor CreateRuleExecutor(IEnumerable<Candle> candles, decimal premium, IDictionary<IEnumerable<Candle>, decimal> assetCashMap, List<Transaction> transactions)
        {
            Func<IRule<IndexedCandle>> buyRule = () =>
            {
                var isPrevBuy = transactions.LastOrDefault(t => t.Candles.Equals(candles))?.Type == TransactionType.Buy;
                return new Rule<IndexedCandle>(ic => !isPrevBuy && _buyRule.IsValid(ic));
            };

            Func<IRule<IndexedCandle>> sellRule = () =>
            {
                var isPrevSell = transactions.LastOrDefault(t => t.Candles.Equals(candles))?.Type == TransactionType.Sell;
                return new Rule<IndexedCandle>(ic => !isPrevSell && _sellRule.IsValid(ic));
            };

            Func<IndexedCandle, int, (TransactionType, IndexedCandle)> outputFunc = (ic, i) =>
            {
                var type = (TransactionType)i;
                if (type.Equals(TransactionType.Buy))
                    BuyAsset(ic, premium, assetCashMap, transactions);
                else
                    SellAsset(ic, premium, assetCashMap, transactions);
                return ((TransactionType)i, ic);
            };

            return new BuySellRuleExecutor(outputFunc, buyRule, sellRule);
        }

        private void BuyAsset(IndexedCandle indexedCandle, decimal premium, IDictionary<IEnumerable<Candle>, decimal> assetCashMap, IList<Transaction> transactions)
        {
            if (assetCashMap.TryGetValue(indexedCandle.BackingList, out decimal cash))
            {
                var nextCandle = indexedCandle.Next;
                int quantity = Convert.ToInt32(Math.Floor((cash - premium) / nextCandle.Open));

                decimal cashOut = nextCandle.Open * quantity + premium;
                assetCashMap[indexedCandle.BackingList] -= cashOut;

                transactions.Add(new Transaction(indexedCandle.BackingList, nextCandle.Index, nextCandle.DateTime, TransactionType.Buy, quantity, cashOut));
                OnBought?.Invoke(indexedCandle.BackingList, nextCandle.Index, nextCandle.DateTime, nextCandle.Open, quantity, cashOut, assetCashMap[indexedCandle.BackingList]);
            }
        }

        private void SellAsset(IndexedCandle indexedCandle, decimal premium, IDictionary<IEnumerable<Candle>, decimal> assetCashMap, IList<Transaction> transactions)
        {
            if (assetCashMap.TryGetValue(indexedCandle.BackingList, out _))
            {
                var nextCandle = indexedCandle.Next;
                var lastTransaction = transactions.LastOrDefault(t => t.Candles.Equals(indexedCandle.BackingList));
                if (lastTransaction.Type == TransactionType.Sell)
                    return;

                decimal cashIn = nextCandle.Open * lastTransaction.Quantity - premium;
                decimal plRatio = (cashIn - lastTransaction.AbsoluteCashFlow) / lastTransaction.AbsoluteCashFlow;
                assetCashMap[indexedCandle.BackingList] += cashIn;

                transactions.Add(new Transaction(indexedCandle.BackingList, nextCandle.Index, nextCandle.DateTime, TransactionType.Sell, lastTransaction.Quantity, cashIn));
                OnSold?.Invoke(indexedCandle.BackingList, nextCandle.Index, nextCandle.DateTime, nextCandle.Open, lastTransaction.Quantity, cashIn, assetCashMap[indexedCandle.BackingList], plRatio);
            }
        }
    }
}