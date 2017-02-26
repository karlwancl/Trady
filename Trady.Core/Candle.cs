using System;
using Trady.Core.Infrastructure;

namespace Trady.Core
{
    public class Candle : TickBase
    {
        public Candle(DateTime dateTime, decimal open, decimal high, decimal low, decimal close, long volume) : base(dateTime)
        {
            Open = open;
            High = high;
            Low = low;
            Close = close;
            Volume = volume;
        }

        public decimal Open { get; private set; }

        public decimal High { get; private set; }

        public decimal Low { get; private set; }

        public decimal Close { get; private set; }

        public long Volume { get; private set; }
    }
}