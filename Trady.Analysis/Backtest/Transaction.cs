using System;
using System.Collections.Generic;
using Trady.Core;
using Trady.Core.Infrastructure;

namespace Trady.Analysis.Backtest
{
    public class Transaction : IEquatable<Transaction>
    {
        public Transaction(IEnumerable<IOhlcv> candles, int index, DateTimeOffset dateTime, TransactionType type, int quantity, decimal absCashFlow)
        {
            IOhlcvDatas = candles;
            Index = index;
            DateTime = dateTime;
            Type = type;
            Quantity = quantity;
            AbsoluteCashFlow = absCashFlow;
        }

        public IEnumerable<IOhlcv> IOhlcvDatas { get; }

        public DateTimeOffset DateTime { get; }

        public int Index { get; }

        public TransactionType Type { get; }

        public int Quantity { get; }

        public decimal AbsoluteCashFlow { get; }

        public bool Equals(Transaction other)
            => IOhlcvDatas.Equals(other.IOhlcvDatas) && Index == other.Index && Type == other.Type && Quantity == other.Quantity && AbsoluteCashFlow == other.AbsoluteCashFlow;
    }
}