using System;
using System.Collections.Generic;
using System.Text;
using Trady.Core.Infrastructure;

namespace Trady.Analysis.Backtest.FeeCalculators
{
    public class InteractiveBrokersFixedFeeCalculator : IFeeCalculator
    {
        public InteractiveBrokersFixedFeeCalculator() : this(0.0075m, 1.0m, 1.0m, 0.0000130m, 0.000119m) { }

        public InteractiveBrokersFixedFeeCalculator(decimal perShareFee, decimal minimumPerOrder, decimal maximumPerOrderPercent, decimal transactionFees, decimal fINRAFees, int roundingDecimals = 2)
        {
            PerShareFee = perShareFee;
            MinimumPerOrder = minimumPerOrder;
            MaximumPerOrderPercent = maximumPerOrderPercent;
            RoundingDecimals = roundingDecimals;
            TransactionFees = transactionFees;
            FINRAFees = fINRAFees;
        }

        protected decimal PerShareFee { get; private set; }
        protected decimal MinimumPerOrder { get; private set; }
        protected decimal MaximumPerOrderPercent { get; private set; }
        protected decimal TransactionFees { get; private set; }
        protected decimal FINRAFees { get; private set; }
        protected int RoundingDecimals { get; private set; }

        public Transaction BuyAsset(IIndexedOhlcv indexedCandle, decimal cash, IIndexedOhlcv nextCandle, bool buyInCompleteQuantities)
        {
            //Interactive Brokers Pricing
            //https://www.interactivebrokers.com/en/index.php?f=1590&p=stocks1

            //determine initial quantity
            decimal quantity = cash / nextCandle.Open;
            var fee = DetermineFee(nextCandle, quantity);
            quantity = (cash - quantity) / nextCandle.Open;

            if(buyInCompleteQuantities)
                quantity = Math.Floor(quantity);

            //get fee after final determination of quantity is found.
            var actualFee = DetermineFee(nextCandle, quantity);

            var costs = RoundingDecimals != -1
                ? Math.Round(actualFee, RoundingDecimals)
                : actualFee;

            decimal cashToBuyAsset = nextCandle.Open * quantity + costs;            

            return new Transaction(indexedCandle.BackingList, nextCandle.Index, nextCandle.DateTime, TransactionType.Buy, quantity, cashToBuyAsset, costs);
        }

        private decimal DetermineFee(IIndexedOhlcv nextCandle, decimal quantity)
        {
            var cost = quantity * PerShareFee;
            var tradePrice = quantity * nextCandle.Open;

            if(cost < 1.0m)
            {
                cost = 1.0m;
            }
            else if(cost / tradePrice > .01m)
            {
                cost = tradePrice * .01m;
            }

            return cost;
        }

        public Transaction SellAsset(IIndexedOhlcv indexedCandle, Transaction lastTransaction)
        {
            var nextCandle = indexedCandle.Next;
            decimal quantity = lastTransaction.Quantity;
            decimal tradeValue = nextCandle.Open * quantity;
            var fees = DetermineTransactionFee(quantity, tradeValue);

            var costs = RoundingDecimals != -1
                ? Math.Round(fees, RoundingDecimals)
                : fees;

            decimal cashWhenSellAsset = nextCandle.Open * quantity - costs;

            cashWhenSellAsset -= costs * cashWhenSellAsset;

            return new Transaction(indexedCandle.BackingList, nextCandle.Index, nextCandle.DateTime, TransactionType.Sell, quantity, cashWhenSellAsset, costs);
        }

        private decimal DetermineTransactionFee(decimal quantity, decimal tradeValue)
        {
            var finraCost = (FINRAFees * quantity);

            if(finraCost > 5.95m)
                finraCost = 5.95m;

            return (tradeValue * TransactionFees) + finraCost;
        }
    }
}
