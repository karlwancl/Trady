using System;
using System.Collections.Generic;
using Trady.Core;

namespace Trady.Analysis.Strategy
{
    public class Transaction : IEquatable<Transaction>
    {
        public Transaction(IEnumerable<Candle> candles, int index, DateTime dateTime, TransactionType type, int quantity, decimal absCashFlow)
        {
            Candles = candles;
            Index = index;
            DateTime = dateTime;
            Type = type;
            Quantity = quantity;
            AbsoluteCashFlow = absCashFlow;
        }

        public IEnumerable<Candle> Candles { get; }

        public DateTime DateTime { get; }

        public int Index { get; }

        public TransactionType Type { get; }

        public int Quantity { get; }

        public decimal AbsoluteCashFlow { get; }

        public bool Equals(Transaction other)
            => Candles.Equals(other.Candles) && Index == other.Index && Type == other.Type && Quantity == other.Quantity && AbsoluteCashFlow == other.AbsoluteCashFlow;
    }
}