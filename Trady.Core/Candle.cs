using System;
using Trady.Core.Infrastructure;

namespace Trady.Core
{
    public class Candle : IOhlcv
    {
        public Candle(DateTimeOffset dateTime, decimal open, decimal high, decimal low, decimal close, decimal volume)
        {
            DateTime = dateTime;
            Open = open;
            High = high;
            Low = low;
            Close = close;
            Volume = volume;
        }

        public DateTimeOffset DateTime { get; }

        public decimal Open { get; }

        public decimal High { get; }

        public decimal Low { get; }

        public decimal Close { get; }

        public decimal Volume { get; }
    }
}