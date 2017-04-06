using System;
using System.Collections.Generic;
using System.Text;
using Trady.Core;

namespace Trady.Analysis.Strategy
{
    public class Transaction : IEquatable<Transaction>
    {
        public Transaction(IList<Candle> candles, int index, DateTime dateTime, TransactionType type, int quantity, decimal absCashFlow)
        {
            Candles = candles;
            Index = index;
            DateTime = dateTime;
            Type = type;
            Quantity = quantity;
            AbsoluteCashFlow = absCashFlow;
        }

        public IList<Candle> Candles { get; private set; }

        public DateTime DateTime { get; private set; }

        public int Index { get; private set; }

        public TransactionType Type { get; private set; }

        public int Quantity { get; private set; }

        public decimal AbsoluteCashFlow { get; private set; }

        public bool Equals(Transaction other)
            => Candles.Equals(other.Candles) && Index == other.Index && Type == other.Type && Quantity == other.Quantity && AbsoluteCashFlow == other.AbsoluteCashFlow;
    }
}
