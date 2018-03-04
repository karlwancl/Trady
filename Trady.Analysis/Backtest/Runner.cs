using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Trady.Analysis.Extension;
using Trady.Core.Infrastructure;

namespace Trady.Analysis.Backtest
{
    public class Runner
    {
        private IDictionary<IEnumerable<IOhlcv>, int> _weightings;
        private Predicate<IIndexedOhlcv> _buyRule, _sellRule;
        private readonly decimal _flatExchangeFee;
        private readonly bool _buyCompleteBaseCurrencies;

        internal Runner(IDictionary<IEnumerable<IOhlcv>, int> weightings,
            Predicate<IIndexedOhlcv> buyRule,
            Predicate<IIndexedOhlcv> sellRule,
            decimal flatExchangeFee,
            bool buyCompleteBaseCurrencies)
        {
            _weightings = weightings;
            _buyRule = buyRule;
            _sellRule = sellRule;
            _flatExchangeFee = flatExchangeFee;
            _buyCompleteBaseCurrencies = buyCompleteBaseCurrencies;
        }

        public event BuyHandler OnBought;

        public delegate void BuyHandler(IEnumerable<IOhlcv> candles, int index, DateTimeOffset dateTime, decimal buyPrice, decimal quantity, decimal absCashFlow, decimal currentCashAmount);

        public event SellHandler OnSold;

        public delegate void SellHandler(IEnumerable<IOhlcv> candles, int index, DateTimeOffset dateTime, decimal sellPrice, decimal quantity, decimal absCashFlow, decimal currentCashAmount, decimal plRatio);

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
                {
                    return null;
                }

                var type = (TransactionType)i;
                if (type.Equals(TransactionType.Buy))
                {
                    BuyAsset(ic, premium, assetCashMap, transactions);
                }
                else
                {
                    SellAsset(ic, premium, assetCashMap, transactions);
                }

                return ((TransactionType)i, ic);
            };

            return new BuySellRuleExecutor(outputFunc, context, buyRule, sellRule);
        }

        private void BuyAsset(IIndexedOhlcv indexedCandle, decimal premium, IDictionary<IEnumerable<IOhlcv>, decimal> assetCashMap, IList<Transaction> transactions)
        {
            if (assetCashMap.TryGetValue(indexedCandle.BackingList, out decimal availableQuoteCurrency))
            {
                var nextIOhlcvData = indexedCandle.Next;
                decimal baseCurrencyQte = (availableQuoteCurrency - premium) / nextIOhlcvData.Open;

                if (_buyCompleteBaseCurrencies)
                {
                    baseCurrencyQte = Math.Floor(baseCurrencyQte);
                }

                decimal quoteCurrencyQte = nextIOhlcvData.Open * baseCurrencyQte + premium;

                // EUR/USD (1€ = 1000$) ; flat exchange fee ratio percent = 0.1
                // you buy 2000$
                // Total 2€, fee = 2 * 0.001 = 0.002, net = 2 - 0.002 = 1.998 €
                baseCurrencyQte -= _flatExchangeFee * baseCurrencyQte;
                var quoteCurrencyFee = _flatExchangeFee * nextIOhlcvData.Open;

                assetCashMap[indexedCandle.BackingList] -= quoteCurrencyQte;
                
                transactions.Add(new Transaction(indexedCandle.BackingList, nextIOhlcvData.Index, nextIOhlcvData.DateTime, TransactionType.Buy, baseCurrencyQte, quoteCurrencyQte));
                OnBought?.Invoke(indexedCandle.BackingList, nextIOhlcvData.Index, nextIOhlcvData.DateTime, nextIOhlcvData.Open, baseCurrencyQte, quoteCurrencyQte, assetCashMap[indexedCandle.BackingList]);
            }
        }

        private void SellAsset(IIndexedOhlcv indexedCandle, decimal premium, IDictionary<IEnumerable<IOhlcv>, decimal> assetCashMap, IList<Transaction> transactions)
        {
            if (assetCashMap.TryGetValue(indexedCandle.BackingList, out _))
            {
                var nextIOhlcvData = indexedCandle.Next;
                var lastTransaction = transactions.LastOrDefault(t => t.IOhlcvDatas.Equals(indexedCandle.BackingList));

                var baseCurrencyQte = lastTransaction.Quantity;
                decimal quoteCurrencyQte = nextIOhlcvData.Open * baseCurrencyQte - premium;
                
                // EUR/USD (1€ = 1000$) ; flat exchange fee ratio percent = 0.1
                // you sell 1.999€
                // Total 1999€, fee = 1.999, net = 1997.001 €
                quoteCurrencyQte -= _flatExchangeFee * quoteCurrencyQte;

                decimal plRatio = (quoteCurrencyQte - lastTransaction.AbsoluteCashFlow) / lastTransaction.AbsoluteCashFlow;
                assetCashMap[indexedCandle.BackingList] += quoteCurrencyQte;

                transactions.Add(new Transaction(indexedCandle.BackingList, nextIOhlcvData.Index, nextIOhlcvData.DateTime, TransactionType.Sell, baseCurrencyQte, quoteCurrencyQte));
                OnSold?.Invoke(indexedCandle.BackingList, nextIOhlcvData.Index, nextIOhlcvData.DateTime, nextIOhlcvData.Open, baseCurrencyQte, quoteCurrencyQte, assetCashMap[indexedCandle.BackingList], plRatio);
            }
        }
    }
}
