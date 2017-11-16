using System;

using Trady.Core.Infrastructure;

namespace Trady.Analysis
{
    public static class Rule
    {
        public static Predicate<IIndexedOhlcv> Create(Predicate<IIndexedOhlcv> predicate) => predicate;

        public static Predicate<IIndexedOhlcv> Or(this Predicate<IIndexedOhlcv> predicate1, Predicate<IIndexedOhlcv> predicate2)
            => ic => predicate1(ic) || predicate2(ic);

        public static Predicate<IIndexedOhlcv> And(this Predicate<IIndexedOhlcv> predicate1, Predicate<IIndexedOhlcv> predicate2)
            => ic => predicate1(ic) && predicate2(ic);
    }
}