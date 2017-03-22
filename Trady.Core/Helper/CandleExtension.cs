using System;

namespace Trady.Core.Helper
{
    public static class CandleExtension
    {
        public static decimal GetUpperShadow(this Candle candle) => candle.Open < candle.Close ? candle.High - candle.Close : candle.High - candle.Open;

        public static decimal GetLowerShadow(this Candle candle) => candle.Open < candle.Close ? candle.Open - candle.Low : candle.Close - candle.Low;

        public static decimal GetBody(this Candle candle) => Math.Abs(candle.Open - candle.Close);

        public static bool IsBullish(this Candle candle) => candle.Open - candle.Close > 0;

        public static bool IsBearish(this Candle candle) => candle.Open - candle.Close < 0;
    }
}