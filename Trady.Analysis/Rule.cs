using System;

using Trady.Core.Infrastructure;

namespace Trady.Analysis
{
    public static class Rule
    {
        public static Predicate<IIndexedOhlcvData> Create(Predicate<IIndexedOhlcvData> predicate) => predicate;

        public static Predicate<IIndexedOhlcvData> Or(this Predicate<IIndexedOhlcvData> predicate1, Predicate<IIndexedOhlcvData> predicate2)
            => ic => predicate1(ic) || predicate2(ic);

        public static Predicate<IIndexedOhlcvData> And(this Predicate<IIndexedOhlcvData> predicate1, Predicate<IIndexedOhlcvData> predicate2)
            => ic => predicate1(ic) && predicate2(ic);
    }
}