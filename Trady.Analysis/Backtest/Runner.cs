using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Trady.Analysis.Extension;
using Trady.Core;
using Trady.Core.Infrastructure;

namespace Trady.Analysis.Backtest
{
    public class Runner
    {
        private IDictionary<IEnumerable<IOhlcv>, int> _weightings;
        private Predicate<IIndexedOhlcv> _buyRule, _sellRule;

        internal Runner(
            IDictionary<IEnumerable<IOhlcv>, int> weightings, 
            Predicate<IIndexedOhlcv> buyRule, 
            Predicate<IIndexedOhlcv> sellRule)
        {
            _weightings = weightings;
            _buyRule = buyRule;
            _sellRule = sellRule;
        }

        public event BuyHandler OnBought;

        public delegate void BuyHandler(IEnumerable<IOhlcv> candles, int index, DateTimeOffset dateTime, decimal buyPrice, int quantity, decimal absCashFlow, decimal currentCashAmount);

        public event SellHandler OnSold;

        public delegate void SellHandler(IEnumerable<IOhlcv> candles, int index, DateTimeOffset dateTime, decimal sellPrice, int quantity, decimal absCashFlow, decimal currentCashAmount, decimal plRatio);

        public Task<Result> RunAsync(decimal principal, decimal premium = 1.0m, DateTime? startTime = null, DateTime? endTime = null)
            => Task.Factory.StartNew(() => Run(principal, premium, startTime, endTime));

        public Result Run(decimal principal, decimal premium = 1.0m, DateTime? startTime = null, DateTime? endTime = null)
        {
            if (_weightings == null || !_weightings.Any())
                throw new ArgumentException("You should have at least one candle set for calculation");

            // Distribute principal to each candle set
            decimal totalWeight = _weightings.Sum(w => w.Value);
            IReadOnlyDictionary<IEnumerable<IOhlcv>, decimal> preAssetCashMap = _weightings.ToDictionary(w => w.Key, w => principal * w.Value / totalWeight);
            var assetCashMap = preAssetCashMap.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

            // Init transaction history
            var transactions = new List<Transaction>();

            // Loop with each asset
            for (int i = 0; i < _weightings.Count; i++)
            {
                var asset = assetCashMap.ElementAt(i).Key;
                var startIndex = asset.FindIndexOrDefault(c => c.DateTime >= (startTime ?? DateTimeOffset.MinValue), 0).Value;
                var endIndex = asset.FindLastIndexOrDefault(c => c.DateTime <= (endTime ?? DateTimeOffset.MaxValue), asset.Count() - 1).Value;
                using (var context = new AnalyzeContext(asset))
                {
                    var executor = CreateBuySellRuleExecutor(context, premium, assetCashMap, transactions);
                    executor.Execute(startIndex, endIndex);
                }
            }

            return new Result(preAssetCashMap, assetCashMap, transactions);
        }

        private BuySellRuleExecutor CreateBuySellRuleExecutor(IAnalyzeContext<IOhlcv> context, decimal premium, IDictionary<IEnumerable<IOhlcv>, decimal> assetCashMap, List<Transaction> transactions)
        {
            Func<IEnumerable<Transaction>, IAnalyzeContext<IOhlcv>, TransactionType, bool> isPreviousTransactionA = (ts, ctx, tt)
                => ts.LastOrDefault(_t => _t.IOhlcvDatas.Equals(ctx.BackingList))?.Type == tt;

            Predicate<IIndexedOhlcv> buyRule = ic
                => !isPreviousTransactionA(transactions, ic.Context, TransactionType.Buy) && _buyRule(ic);

            Predicate<IIndexedOhlcv> sellRule = ic
                => transactions.Any() && !isPreviousTransactionA(transactions, ic.Context, TransactionType.Sell) && _sellRule(ic);

            Func<IIndexedOhlcv, int, (TransactionType, IIndexedOhlcv)?> outputFunc = (ic, i) =>
            {
                if (ic.Next == null)
                    return null;

                var type = (TransactionType)i;
                if (type.Equals(TransactionType.Buy))
                    BuyAsset(ic, premium, assetCashMap, transactions);
                else
                    SellAsset(ic, premium, assetCashMap, transactions);
                return ((TransactionType)i, ic);
            };

            return new BuySellRuleExecutor(outputFunc, context, buyRule, sellRule);
        }

        private void BuyAsset(IIndexedOhlcv indexedCandle, decimal premium, IDictionary<IEnumerable<IOhlcv>, decimal> assetCashMap, IList<Transaction> transactions)
        {
            if (assetCashMap.TryGetValue(indexedCandle.BackingList, out decimal cash))
            {
                var nextIOhlcvData = indexedCandle.Next;
                int quantity = Convert.ToInt32(Math.Floor((cash - premium) / nextIOhlcvData.Open));
                
                decimal cashOut = nextIOhlcvData.Open * quantity + premium;
                assetCashMap[indexedCandle.BackingList] -= cashOut;

                transactions.Add(new Transaction(indexedCandle.BackingList, nextIOhlcvData.Index, nextIOhlcvData.DateTime, TransactionType.Buy, quantity, cashOut));
                OnBought?.Invoke(indexedCandle.BackingList, nextIOhlcvData.Index, nextIOhlcvData.DateTime, nextIOhlcvData.Open, quantity, cashOut, assetCashMap[indexedCandle.BackingList]);
            }
        }

        private void SellAsset(IIndexedOhlcv indexedCandle, decimal premium, IDictionary<IEnumerable<IOhlcv>, decimal> assetCashMap, IList<Transaction> transactions)
        {
            if (assetCashMap.TryGetValue(indexedCandle.BackingList, out _))
            {
                var nextIOhlcvData = indexedCandle.Next;
                var lastTransaction = transactions.LastOrDefault(t => t.IOhlcvDatas.Equals(indexedCandle.BackingList));

                decimal cashIn = nextIOhlcvData.Open * lastTransaction.Quantity - premium;
                decimal plRatio = (cashIn - lastTransaction.AbsoluteCashFlow) / lastTransaction.AbsoluteCashFlow;
                assetCashMap[indexedCandle.BackingList] += cashIn;

                transactions.Add(new Transaction(indexedCandle.BackingList, nextIOhlcvData.Index, nextIOhlcvData.DateTime, TransactionType.Sell, lastTransaction.Quantity, cashIn));
                OnSold?.Invoke(indexedCandle.BackingList, nextIOhlcvData.Index, nextIOhlcvData.DateTime, nextIOhlcvData.Open, lastTransaction.Quantity, cashIn, assetCashMap[indexedCandle.BackingList], plRatio);
            }
        }
    }
}