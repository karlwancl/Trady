using System;

using AnTick = Trady.Analysis.AnalyzableTick<decimal?>;
using AnTp2Tick = Trady.Analysis.AnalyzableTick<(decimal?, decimal?)>;
using AnTp3Tick = Trady.Analysis.AnalyzableTick<(decimal?, decimal?, decimal?)>;

namespace Trady.Analysis.Extension
{
    public static class PredicateExtension
    {
        public static bool IsTrue<T>(this T? obj, Predicate<T> predicate) where T : struct
            => obj.HasValue && predicate(obj.Value);

        public static bool IsTrue<T>(this (T?, T?) obj, Func<T, T, bool> predicate) where T : struct
            => obj.Item1.HasValue && obj.Item2.HasValue && predicate(obj.Item1.Value, obj.Item2.Value);

        public static bool IsTrue<T>(this (T?, T?, T?) obj, Func<T, T, T?, bool> predicate) where T : struct
            => obj.Item1.HasValue && obj.Item2.HasValue && predicate(obj.Item1.Value, obj.Item2.Value, obj.Item3);

        public static bool IsTrue(this (AnTick, AnTick, AnTick) obj, Func<AnTick, AnTick, AnTick, bool> predicate)
        {
            bool isValid(AnTick t) => t != null && t.Tick.HasValue;
            return isValid(obj.Item1) && isValid(obj.Item2) && predicate(obj.Item1, obj.Item2, obj.Item3);
        }

        public static bool IsTrue(this (AnTp2Tick, AnTp2Tick, AnTp2Tick) obj, Func<AnTp2Tick, AnTp2Tick, AnTp2Tick, bool> predicate)
        {
            bool isValid(AnTp2Tick t) => t != null && !default((decimal?, decimal?)).Equals(t.Tick) && t.Tick.Item1.HasValue && t.Tick.Item2.HasValue;
            return isValid(obj.Item1) && isValid(obj.Item2) && predicate(obj.Item1, obj.Item2, obj.Item3);
        }

        public static bool IsTrue(this (AnTp3Tick, AnTp3Tick, AnTp3Tick) obj, Func<AnTp3Tick, AnTp3Tick, AnTp3Tick, bool> predicate)
        {
            bool isValid(AnTp3Tick t) => t != null && !default((decimal?, decimal?, decimal?)).Equals(t.Tick) && t.Tick.Item1.HasValue && t.Tick.Item2.HasValue && t.Tick.Item3.HasValue;
            return isValid(obj.Item1) && isValid(obj.Item2) && predicate(obj.Item1, obj.Item2, obj.Item3);
        }

        public static bool IsPositive(this decimal? obj)
            => IsTrue(obj, o => o > 0);

        public static bool IsPositive(this decimal? obj, Func<decimal?, decimal?> mapper)
            => IsPositive(mapper(obj));

        public static bool IsNegative(this decimal? obj)
            => IsTrue(obj, o => o < 0);

        public static bool IsNegative(this decimal? obj, Func<decimal?, decimal?> mapper)
            => IsNegative(mapper(obj));
    }
}
