using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Trady.Analysis.Backtest.FeeCalculators;
using Trady.Analysis.Extension;
using Trady.Core.Infrastructure;

namespace Trady.Analysis.Backtest
{
    public class Runner
    {
        private IDictionary<IEnumerable<IOhlcv>, int> _weightings;
        private Predicate<IIndexedOhlcv> _buyRule, _sellRule;
        private readonly bool _buyInCompleteQuantity;
        private readonly IFeeCalculator _calculator;

        internal Runner(IDictionary<IEnumerable<IOhlcv>, int> weightings,
            Predicate<IIndexedOhlcv> buyRule,
            Predicate<IIndexedOhlcv> sellRule,
            bool buyInCompleteQuantity,
            IFeeCalculator calculator)
        {
            _weightings = weightings;
            _buyRule = buyRule;
            _sellRule = sellRule;
            _buyInCompleteQuantity = buyInCompleteQuantity;
            _calculator = calculator;
        }

        public event BuyHandler OnBought;

        public delegate void BuyHandler(IEnumerable<IOhlcv> candles, int index, DateTimeOffset dateTime, decimal buyPrice, decimal quantity, decimal absCashFlow, decimal currentCashAmount);

        public event SellHandler OnSold;

        public delegate void SellHandler(IEnumerable<IOhlcv> candles, int index, DateTimeOffset dateTime, decimal sellPrice, decimal quantity, decimal absCashFlow, decimal currentCashAmount, decimal plRatio);

        public Task<Result> RunAsync(decimal principal, DateTime? startTime = null, DateTime? endTime = null)
            => Task.Factory.StartNew(() => Run(principal, startTime, endTime));

        public Result Run(decimal principal, DateTime? startTime = null, DateTime? endTime = null)
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
                    var executor = CreateBuySellRuleExecutor(context, _calculator, assetCashMap, transactions);
                    executor.Execute(startIndex, endIndex);
                }
            }

            return new Result(preAssetCashMap, assetCashMap, transactions);
        }

        private BuySellRuleExecutor CreateBuySellRuleExecutor(IAnalyzeContext<IOhlcv> context, IFeeCalculator calculator, IDictionary<IEnumerable<IOhlcv>, decimal> assetCashMap, List<Transaction> transactions)
        {
            bool isPrevTransactionOfType(IEnumerable<Transaction> ts, IAnalyzeContext<IOhlcv> ctx, TransactionType tt)
                => ts.LastOrDefault(_t => _t.OhlcvList.Equals(ctx.BackingList))?.Type == tt;

            bool buyRule(IIndexedOhlcv ic)
                => !isPrevTransactionOfType(transactions, ic.Context, TransactionType.Buy) && _buyRule(ic);

            bool sellRule(IIndexedOhlcv ic)
                => transactions.Any() && !isPrevTransactionOfType(transactions, ic.Context, TransactionType.Sell) && _sellRule(ic);

            (TransactionType, IIndexedOhlcv)? outputFunc(IIndexedOhlcv ic, int i)
            {
                if (ic.Next == null)
                    return null;

                var type = (TransactionType)i;
                if (type.Equals(TransactionType.Buy))
                    BuyAsset(ic, calculator, assetCashMap, transactions);
                else
                    SellAsset(ic, calculator, assetCashMap, transactions);

                return ((TransactionType)i, ic);
            }

            return new BuySellRuleExecutor(outputFunc, context, buyRule, sellRule);
        }

        private void BuyAsset(IIndexedOhlcv indexedCandle, IFeeCalculator calculator, IDictionary<IEnumerable<IOhlcv>, decimal> assetCashMap, IList<Transaction> transactions)
        {
            if (assetCashMap.TryGetValue(indexedCandle.BackingList, out decimal cash))
            {
                var nextCandle = indexedCandle.Next;
                
                //Use calculator to determine transaction quantities, costs, etc.
                var transaction = calculator.BuyAsset(nextCandle, cash, nextCandle, _buyInCompleteQuantity);

                assetCashMap[nextCandle.BackingList] -= transaction.AbsoluteCashFlow; //cash to buy asset

                transactions.Add(transaction);
                OnBought?.Invoke(indexedCandle.BackingList, nextCandle.Index, nextCandle.DateTime, nextCandle.Open, transaction.Quantity, transaction.AbsoluteCashFlow, assetCashMap[indexedCandle.BackingList]);
            }
        }        

        private void SellAsset(IIndexedOhlcv indexedCandle, IFeeCalculator calculator, IDictionary<IEnumerable<IOhlcv>, decimal> assetCashMap, IList<Transaction> transactions)
        {
            if (assetCashMap.TryGetValue(indexedCandle.BackingList, out _))
            {
                var nextCandle = indexedCandle.Next;
                var lastTransaction = transactions.LastOrDefault(t => t.OhlcvList.Equals(indexedCandle.BackingList));
                if (lastTransaction == default)
                    return;
                
                var transaction = calculator.SellAsset(indexedCandle, lastTransaction);

                assetCashMap[indexedCandle.BackingList] += transaction.AbsoluteCashFlow;
                decimal profitLossRatio = (transaction.AbsoluteCashFlow - lastTransaction.AbsoluteCashFlow) / lastTransaction.AbsoluteCashFlow;

                transactions.Add(transaction);
                OnSold?.Invoke(indexedCandle.BackingList, nextCandle.Index, nextCandle.DateTime, nextCandle.Open, transaction.Quantity, transaction.AbsoluteCashFlow, assetCashMap[indexedCandle.BackingList],  profitLossRatio);
            }
        }
    }
}
