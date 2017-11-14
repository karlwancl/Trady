using System;
using System.Collections.Generic;
using Trady.Analysis.Infrastructure;
using Trady.Core;
using Trady.Core.Infrastructure;

namespace Trady.Analysis
{
    public class SimpleRuleExecutor : RuleExecutorBase<IOhlcv, IIndexedOhlcv, IIndexedOhlcv>
    {
        public SimpleRuleExecutor(IAnalyzeContext<IOhlcv> context, Predicate<IIndexedOhlcv> rule)
            : base((l, i) => l, context, new[] { rule })
        {
        }

        public override Func<IEnumerable<IOhlcv>, int, IIndexedOhlcv> IndexedObjectConstructor
            => (l, i) => new IndexedCandle(l, i);
    }
}