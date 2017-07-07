using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Trady.Analysis.Helper;
using Trady.Analysis.Strategy.Rule;
using Trady.Core;

namespace Trady.Analysis.Strategy
{
    public class Portfolio
    {
        IDictionary<IEnumerable<Candle>, int> _weightings;

        IRule<IndexedCandle> _buyRule;
        IRule<IndexedCandle> _sellRule;

        public event BuyHandler OnBought;

        public delegate void BuyHandler(IEnumerable<Candle> candles, int index, DateTime dateTime, decimal buyPrice, int quantity, decimal absCashFlow, decimal currentCashAmount);

        public event SellHandler OnSold;

        public delegate void SellHandler(IEnumerable<Candle> candles, int index, DateTime dateTime, decimal sellPrice, int quantity, decimal absCashFlow, decimal currentCashAmount, decimal plRatio);

        #region Builder

        public Portfolio() : this(null, null, null)
        {
        }

        Portfolio(IDictionary<IEnumerable<Candle>, int> weightings, IRule<IndexedCandle> buyRule, IRule<IndexedCandle> sellRule)
        {
            _weightings = weightings ?? new Dictionary<IEnumerable<Candle>, int>();
            _buyRule = buyRule;
            _sellRule = sellRule;
        }

        public Portfolio Add(IEnumerable<Candle> candles, int weighting = 1)
        {
            _weightings.Add(candles, weighting);
            return new Portfolio(_weightings, _buyRule, _sellRule);
        }

        public Portfolio Buy(IRule<IndexedCandle> rule)
            => new Portfolio(_weightings, (_buyRule?.Or(rule)) ?? rule, _sellRule);

        public Portfolio Sell(IRule<IndexedCandle> rule)
            => new Portfolio(_weightings, _buyRule, (_sellRule?.Or(rule)) ?? rule);

        #endregion Builder

        public async Task<Result> RunBacktestAsync(decimal principal, decimal premium = 1.0m, DateTime? startTime = null, DateTime? endTime = null)
            => await Task.Factory.StartNew(() => RunBacktest(principal, premium, startTime, endTime));

        public Result RunBacktest(decimal principal, decimal premium = 1.0m, DateTime? startTime = null, DateTime? endTime = null)
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

                for (int j = startIndex; j <= endIndex; j++)
                {
                    var indexedCandle = new IndexedCandle(asset, j);
                    var lastTransaction = transactions.LastOrDefault(t => t.Candles.Equals(asset));
                    if (lastTransaction?.Type != TransactionType.Buy && _buyRule.IsValid(indexedCandle))
                        BuyAsset(indexedCandle, premium, assetCashMap, transactions);
                    else if (lastTransaction?.Type != TransactionType.Sell && _sellRule.IsValid(indexedCandle))
                        SellAsset(indexedCandle, premium, assetCashMap, transactions);
                }
            }

            return new Result(preAssetCashMap, assetCashMap, transactions);
        }

        void BuyAsset(IndexedCandle indexedCandle, decimal premium, IDictionary<IEnumerable<Candle>, decimal> assetCashMap, IList<Transaction> transactions)
        {
            if (assetCashMap.TryGetValue(indexedCandle.Candles, out decimal cash))
            {
                var nextCandle = indexedCandle.Next;
                int quantity = Convert.ToInt32(Math.Floor((cash - premium) / nextCandle.Open));

                decimal cashOut = nextCandle.Open * quantity + premium;
                assetCashMap[indexedCandle.Candles] -= cashOut;

                transactions.Add(new Transaction(indexedCandle.Candles, nextCandle.Index, nextCandle.DateTime, TransactionType.Buy, quantity, cashOut));
                OnBought?.Invoke(indexedCandle.Candles, nextCandle.Index, nextCandle.DateTime, nextCandle.Open, quantity, cashOut, assetCashMap[indexedCandle.Candles]);
            }
        }

        void SellAsset(IndexedCandle indexedCandle, decimal premium, IDictionary<IEnumerable<Candle>, decimal> assetCashMap, IList<Transaction> transactions)
        {
            if (assetCashMap.TryGetValue(indexedCandle.Candles, out _))
            {
                var nextCandle = indexedCandle.Next;
                var lastTransaction = transactions.LastOrDefault(t => t.Candles.Equals(indexedCandle.Candles));
                if (lastTransaction.Type == TransactionType.Sell)
                    return;

                decimal cashIn = nextCandle.Open * lastTransaction.Quantity - premium;
                decimal plRatio = (cashIn - lastTransaction.AbsoluteCashFlow) / lastTransaction.AbsoluteCashFlow;
                assetCashMap[indexedCandle.Candles] += cashIn;

                transactions.Add(new Transaction(indexedCandle.Candles, nextCandle.Index, nextCandle.DateTime, TransactionType.Sell, lastTransaction.Quantity, cashIn));
                OnSold?.Invoke(indexedCandle.Candles, nextCandle.Index, nextCandle.DateTime, nextCandle.Open, lastTransaction.Quantity, cashIn, assetCashMap[indexedCandle.Candles], plRatio);
            }
        }
    }
}