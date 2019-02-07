using System;
using System.Runtime.Serialization;
using Trady.Core.Infrastructure;

namespace Trady.Analysis.Backtest.FeeCalculators
{
    /// <summary>
    /// Base calculator for any rule.
    /// </summary>
    /// <remarks>
    /// You can implement your own calculator for any particular broker's rules.
    /// For example: some brokers charge differently for larger orders...  You can implement that logic in in your own IAssetCalculator
    /// </remarks>
    public class FeeCalculator : IFeeCalculator
    {
        public FeeCalculator() : this(0, 1)
        {

        }
        public FeeCalculator(decimal flatExchangeFee, decimal premium, int roundingDecimals = -1)
        {
            FlatExchangeFee = flatExchangeFee;
            Premium = premium;
            if(roundingDecimals != -1)
            {
                RoundingDecimals = roundingDecimals;
            }
        }

        protected decimal FlatExchangeFee { get; private set; }
        protected decimal Premium { get; private set; }
        protected int RoundingDecimals { get; private set; }

        public Transaction BuyAsset(IIndexedOhlcv indexedCandle, decimal cash, IIndexedOhlcv nextCandle, bool buyInCompleteQuantities)
        {
            ValidateProperties();

            decimal quantity = (cash - Premium) / nextCandle.Open;
            if(buyInCompleteQuantities)
                quantity = Math.Floor(quantity);

            decimal cashToBuyAsset = nextCandle.Open * quantity + Premium;

            var costs = RoundingDecimals != -1
                ? Math.Round(Premium + (FlatExchangeFee * quantity), RoundingDecimals)
                : Premium + (FlatExchangeFee * quantity);

            // EUR/USD (1€ = 1000$) ; flat exchange fee ratio percent = 0.1
            // you buy 2000$
            // Total 2€, fee = 2 * 0.001 = 0.002, net = 2 - 0.002 = 1.998 €
            quantity -= FlatExchangeFee * quantity;
            //var quoteCurrencyFee = _flatExchangeFee * nextCandle.Open;

            return new Transaction(indexedCandle.BackingList, nextCandle.Index, nextCandle.DateTime, TransactionType.Buy, quantity, cashToBuyAsset, costs);
        }

       

        public Transaction SellAsset(IIndexedOhlcv indexedCandle, Transaction lastTransaction)
        {
            ValidateProperties();

            var nextCandle = indexedCandle.Next;
            
            decimal quantity = lastTransaction.Quantity;
            decimal cashWhenSellAsset = nextCandle.Open * quantity - Premium;

            // EUR/USD (1€ = 1000$) ; flat exchange fee ratio percent = 0.1
            // you sell 1.999€
            // Total 1999€, fee = 1.999, net = 1997.001 €
            var costs = RoundingDecimals != -1
                ? Math.Round(Premium + (FlatExchangeFee * cashWhenSellAsset), RoundingDecimals)
                : Premium + (FlatExchangeFee * cashWhenSellAsset);

            cashWhenSellAsset -= FlatExchangeFee * cashWhenSellAsset;         

            return new Transaction(indexedCandle.BackingList, nextCandle.Index, nextCandle.DateTime, TransactionType.Sell, quantity, cashWhenSellAsset, costs);
        }

        private void ValidateProperties()
        {
            if(FlatExchangeFee < 0 || FlatExchangeFee >= 1)
            {
                throw new FeeCalculationException("The FlatExchangeFee must be set to a value that is greater than equal to zero (>= 0) and less than one (< 1).  Example => .25");
            }
        }
    }

    public class FeeCalculationException : Exception
    {
        public FeeCalculationException() : base() { }
        public FeeCalculationException(string message) : base(message) { }
        public FeeCalculationException(string message, Exception innerException) : base(message, innerException) { }
        public FeeCalculationException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
