using System;
using System.Collections.Generic;
using System.Text;
using Trady.Core;
using System.Linq;

namespace Trady.Analysis.Strategy
{
    public class Result
    {
        public Result(IReadOnlyDictionary<IList<Candle>, decimal> preAssetCashMap, IDictionary<IList<Candle>, decimal> postAssetCashMap, IList<Transaction> transactions)
        {
            PreAssetCashMap = preAssetCashMap;
            PostAssetCashMap = postAssetCashMap;
            Transactions = transactions;
        }

        public IReadOnlyDictionary<IList<Candle>, decimal> PreAssetCashMap { get; private set; }

        public IDictionary<IList<Candle>, decimal> PostAssetCashMap { get; private set; }

        public IList<Transaction> Transactions { get; private set; }

        #region Count

        public int TotalTransactionCount => Transactions.Count();

        public int TotalBuyCount => Transactions.Count(t => t.Type == TransactionType.Buy);

        public int TotalSellCount => Transactions.Count(t => t.Type == TransactionType.Sell);

        public int BuyCount(IList<Candle> candles) => Transactions.Count(t => t.Candles.Equals(candles) && t.Type == TransactionType.Buy);

        public int SellCount(IList<Candle> candles) => Transactions.Count(t => t.Candles.Equals(candles) && t.Type == TransactionType.Sell);

        public int TotalCorrectedTransactionCount => TotalCorrectedBuyCount + TotalSellCount;

        public int TotalCorrectedBuyCount => PreAssetCashMap.Select(ac => ac.Key).Sum(a => CorrectedBuyCount(a));

        public int CorrectedBuyCount(IList<Candle> candles) 
        {
            var trans = Transactions.Where(t => t.Candles.Equals(candles));
            if (trans.Any())
            {
                var lastTrans = trans.ElementAt(trans.Count() - 1);
                return trans.Count(t => t.Type == TransactionType.Buy) - (lastTrans.Type == TransactionType.Buy ? 1 : 0);
            }
            return 0;
        }

        #endregion

        #region Sum

        public decimal TotalCorrectedProfitLossRatio => TotalCorrectedProfitLoss / TotalPrincipal;

        public decimal TotalCorrectedProfitLoss => TotalCorrectedBalance - TotalPrincipal;

        public decimal TotalCorrectedBalance => PreAssetCashMap.Select(ac => ac.Key).Sum(a => CorrectedBalance(a));

        public decimal TotalPrincipal => PreAssetCashMap.Select(ac => ac.Key).Sum(a => Principal(a));

        public decimal CorrectedProfitLossRatio(IList<Candle> candles) => CorrectedProfitLoss(candles) / Principal(candles);

        public decimal CorrectedProfitLoss(IList<Candle> candles) => CorrectedBalance(candles) - Principal(candles);

        public decimal CorrectedBalance(IList<Candle> candles)
        {
            if (!PostAssetCashMap.TryGetValue(candles, out decimal postCash))
                throw new ArgumentException("Can't get the final cash amount for the corresponding asset!");

            var trans = Transactions.Where(t => t.Candles.Equals(candles));
            if (trans.Any() && trans.Last().Type == TransactionType.Buy)
                postCash += trans.Last().AbsoluteCashFlow;

            return postCash;
        }

        public decimal Principal(IList<Candle> candles)
        {
            if (!PreAssetCashMap.TryGetValue(candles, out decimal initial))
                throw new ArgumentException("Can't get the final cash amount for the corresponding asset!");

            return initial;
        }

        #endregion  
    }
}
