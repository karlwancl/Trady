using System;
namespace Trady.Analysis.Helper
{
    public static class PublicHelper
    {
        public static bool IsTrue<T>(this T? obj, Predicate<T> predicate) where T : struct
            => obj.HasValue && predicate(obj.Value);
    }
}
