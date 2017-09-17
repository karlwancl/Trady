using System;
namespace Trady.Analysis.Helper
{
    public static class PublicHelper
    {
        public static bool IsTrue<T>(this T? obj, Predicate<T> predicate) where T : struct
            => obj.HasValue && predicate(obj.Value);

        public static bool IsPositive(this decimal? obj)
            => IsTrue(obj, o => o > 0);

        public static bool IsNegative(this decimal? obj)
            => IsTrue(obj, o => o < 0);
    }
}
