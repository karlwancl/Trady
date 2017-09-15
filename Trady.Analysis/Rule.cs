using System;

namespace Trady.Analysis
{
    public static class Rule
    {
        public static Predicate<IndexedCandle> Create(Predicate<IndexedCandle> predicate) => predicate;

        public static Predicate<IndexedCandle> Or(this Predicate<IndexedCandle> predicate1, Predicate<IndexedCandle> predicate2)
            => ic => predicate1(ic) || predicate2(ic);

        public static Predicate<IndexedCandle> And(this Predicate<IndexedCandle> predicate1, Predicate<IndexedCandle> predicate2)
            => ic => predicate1(ic) && predicate2(ic);
    }
}