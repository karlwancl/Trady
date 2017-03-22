using System;

namespace Trady.Test
{
    internal static class Helper
    {
        public static bool IsApproximatelyEquals(this decimal expected, decimal actual)
        {
            var error = Math.Abs((actual - expected) / expected);
            return error < 0.05m;
        }
    }
}