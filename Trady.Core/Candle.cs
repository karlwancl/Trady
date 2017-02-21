using System;

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

        public decimal UpperShadow => Open < Close ? High - Close : High - Open;

        public decimal LowerShadow => Open < Close ? Open - Low : Close - Low;

        public decimal Body => Math.Abs(Open - Close);

        public bool IsBullish => Open - Close > 0;

        public bool IsBearish => Open - Close < 0;
    }
}