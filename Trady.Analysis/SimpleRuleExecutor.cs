using System;
using System.Collections.Generic;
using Trady.Analysis.Infrastructure;
using Trady.Core;
using Trady.Core.Infrastructure;

namespace Trady.Analysis
{
    public class SimpleRuleExecutor : RuleExecutorBase<IOhlcvData, IIndexedOhlcvData, IIndexedOhlcvData>
    {
        public SimpleRuleExecutor(IAnalyzeContext<IOhlcvData> context, Predicate<IIndexedOhlcvData> rule)
            : base((l, i) => l, context, new[] { rule })
        {
        }

        public override Func<IEnumerable<IOhlcvData>, int, IIndexedOhlcvData> IndexedObjectConstructor
            => (l, i) => new IndexedCandle(l, i);
    }
}