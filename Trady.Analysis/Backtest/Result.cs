using System;
using System.Collections.Generic;
using System.Linq;
using Trady.Core;
using Trady.Core.Infrastructure;

namespace Trady.Analysis.Backtest
{
    public class Result
    {
        internal Result(IReadOnlyDictionary<IEnumerable<IOhlcv>, decimal> preAssetCashMap, IDictionary<IEnumerable<IOhlcv>, decimal> postAssetCashMap, IList<Transaction> transactions)
        {
            PreAssetCashMap = preAssetCashMap;
            PostAssetCashMap = postAssetCashMap;
            Transactions = transactions;
        }

        public IReadOnlyDictionary<IEnumerable<IOhlcv>, decimal> PreAssetCashMap { get; }

        public IDictionary<IEnumerable<IOhlcv>, decimal> PostAssetCashMap { get; }

        public IEnumerable<Transaction> Transactions { get; }

        #region Count

        public int TotalTransactionCount => Transactions.Count();

        public int TotalBuyCount => Transactions.Count(t => t.Type == TransactionType.Buy);

        public int TotalSellCount => Transactions.Count(t => t.Type == TransactionType.Sell);

        public int BuyCount(IEnumerable<IOhlcv> candles) => Transactions.Count(t => t.IOhlcvDatas.Equals(candles) && t.Type == TransactionType.Buy);

        public int SellCount(IEnumerable<IOhlcv> candles) => Transactions.Count(t => t.IOhlcvDatas.Equals(candles) && t.Type == TransactionType.Sell);

        public int TotalCorrectedTransactionCount => TotalCorrectedBuyCount + TotalSellCount;

        public int TotalCorrectedBuyCount => PreAssetCashMap.Select(ac => ac.Key).Sum(a => CorrectedBuyCount(a));

        public int CorrectedBuyCount(IEnumerable<IOhlcv> candles)
        {
            var trans = Transactions.Where(t => t.IOhlcvDatas.Equals(candles));
            if (trans.Any())
            {
                var lastTrans = trans.ElementAt(trans.Count() - 1);
                return trans.Count(t => t.Type == TransactionType.Buy) - (lastTrans.Type == TransactionType.Buy ? 1 : 0);
            }
            return 0;
        }

        #endregion Count

        #region Sum

        public decimal TotalCorrectedProfitLossRatio => TotalCorrectedProfitLoss / TotalPrincipal;

        public decimal TotalCorrectedProfitLoss => TotalCorrectedBalance - TotalPrincipal;

        public decimal TotalCorrectedBalance => PreAssetCashMap.Select(ac => ac.Key).Sum(a => CorrectedBalance(a));

        public decimal TotalPrincipal => PreAssetCashMap.Select(ac => ac.Key).Sum(a => Principal(a));

        public decimal CorrectedProfitLossRatio(IEnumerable<IOhlcv> candles) => CorrectedProfitLoss(candles) / Principal(candles);

        public decimal CorrectedProfitLoss(IEnumerable<IOhlcv> candles) => CorrectedBalance(candles) - Principal(candles);

        public decimal CorrectedBalance(IEnumerable<IOhlcv> candles)
        {
            if (!PostAssetCashMap.TryGetValue(candles, out decimal postCash))
                throw new ArgumentException("Can't get the final cash amount for the corresponding asset!");

            var trans = Transactions.Where(t => t.IOhlcvDatas.Equals(candles));
            if (trans.Any() && trans.Last().Type == TransactionType.Buy)
                postCash += trans.Last().AbsoluteCashFlow;

            return postCash;
        }

        public decimal Principal(IEnumerable<IOhlcv> candles)
        {
            if (!PreAssetCashMap.TryGetValue(candles, out decimal initial))
                throw new ArgumentException("Can't get the final cash amount for the corresponding asset!");

            return initial;
        }

        #endregion Sum
    }
}
