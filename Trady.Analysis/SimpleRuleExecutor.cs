using System;
using System.Collections.Generic;
using Trady.Analysis.Infrastructure;
using Trady.Core;
using Trady.Core.Infrastructure;

namespace Trady.Analysis
{
    public class SimpleRuleExecutor : RuleExecutorBase<Candle, IndexedCandle, IndexedCandle>
    {
        public SimpleRuleExecutor(IAnalyzeContext<Candle> context, Predicate<IndexedCandle> rule)
            : base((l, i) => l, context, new Predicate<IndexedCandle>[] { rule })
        {
        }

        public override Func<IEnumerable<Candle>, int, IndexedCandle> IndexedObjectConstructor
            => (l, i) => new IndexedCandle(l, i);
    }
}