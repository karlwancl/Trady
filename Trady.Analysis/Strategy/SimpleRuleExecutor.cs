using System;
using System.Collections.Generic;
using Trady.Analysis.Strategy.Rule;
using Trady.Core;

namespace Trady.Analysis.Strategy
{
    public class SimpleRuleExecutor : RuleExecutorBase<Candle, IndexedCandle, IndexedCandle>
    {
        public SimpleRuleExecutor(IRule<IndexedCandle> rule)
            : base((l, i) => l, () => rule)
        {
        }

        public override Func<IEnumerable<Candle>, int, IndexedCandle> IndexedObjectConstructor
            => (l, i) => new IndexedCandle(l, i);
    }
}