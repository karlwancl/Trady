using System;
namespace Trady.Analysis.Indicator
{
    public static class Smoothing
    {
        public static Func<int, decimal> Ema(int period) => i => 2.0m / (period + 1);

        public static Func<int, decimal> Mma(int period) => i => 1.0m / period;
    }
}
