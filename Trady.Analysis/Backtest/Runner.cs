using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Trady.Core;
using Trady.Core.Infrastructure;

namespace Trady.Analysis.Backtest
{
    public class Runner
    {
        private IDictionary<IEnumerable<Candle>, int> _weightings;
        private Predicate<IndexedCandle> _buyRule, _sellRule;

        internal Runner(
            IDictionary<IEnumerable<Candle>, int> weightings, 
            Predicate<IndexedCandle> buyRule, 
            Predicate<IndexedCandle> sellRule)
        {
            _weightings = weightings;
            _buyRule = buyRule;
            _sellRule = sellRule;
        }

        public event BuyHandler OnBought;

        public delegate void BuyHandler(IEnumerable<Candle> candles, int index, DateTime dateTime, decimal buyPrice, int quantity, decimal absCashFlow, decimal currentCashAmount);

        public event SellHandler OnSold;

        public delegate void SellHandler(IEnumerable<Candle> candles, int index, DateTime dateTime, decimal sellPrice, int quantity, decimal absCashFlow, decimal currentCashAmount, decimal plRatio);

        public Task<Result> RunAsync(decimal principal, decimal premium = 1.0m, DateTime? startTime = null, DateTime? endTime = null)
            => Task.Factory.StartNew(() => Run(principal, premium, startTime, endTime));

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
                using (var context = new AnalyzeContext(asset))
                {
                    var executor = CreateBuySellRuleExecutor(context, premium, assetCashMap, transactions);
                    executor.Execute(startIndex, endIndex);
                }
            }

            return new Result(preAssetCashMap, assetCashMap, transactions);
        }

        private BuySellRuleExecutor CreateBuySellRuleExecutor(IAnalyzeContext<Candle> context, decimal premium, IDictionary<IEnumerable<Candle>, decimal> assetCashMap, List<Transaction> transactions)
        {
            Func<IEnumerable<Transaction>, IAnalyzeContext<Candle>, TransactionType, bool> isPreviousTransactionA = (ts, ctx, tt)
                => ts.LastOrDefault(_t => _t.Candles.Equals(ctx.BackingList))?.Type == tt;

            Predicate<IndexedCandle> buyRule = ic
                => !isPreviousTransactionA(transactions, ic.Context, TransactionType.Buy) && _buyRule(ic);

            Predicate<IndexedCandle> sellRule = ic
                => !isPreviousTransactionA(transactions, ic.Context, TransactionType.Sell) && _sellRule(ic);

            Func<IndexedCandle, int, (TransactionType, IndexedCandle)> outputFunc = (ic, i) =>
            {
                var type = (TransactionType)i;
                if (type.Equals(TransactionType.Buy))
                    BuyAsset(ic, premium, assetCashMap, transactions);
                else
                    SellAsset(ic, premium, assetCashMap, transactions);
                return ((TransactionType)i, ic);
            };

            return new BuySellRuleExecutor(outputFunc, context, buyRule, sellRule);
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