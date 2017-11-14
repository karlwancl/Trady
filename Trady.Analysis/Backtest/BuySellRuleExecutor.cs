using System;
using System.Collections.Generic;
using Trady.Analysis.Infrastructure;
using Trady.Core;
using Trady.Core.Infrastructure;

namespace Trady.Analysis.Backtest
{
    public class BuySellRuleExecutor : RuleExecutorBase<IOhlcv, IIndexedOhlcv, (TransactionType, IIndexedOhlcv)?>
    {
        public BuySellRuleExecutor(
            Func<IIndexedOhlcv, int, (TransactionType, IIndexedOhlcv)?> outputFunc, 
            IAnalyzeContext<IOhlcv> context, 
            Predicate<IIndexedOhlcv> buyRule, 
            Predicate<IIndexedOhlcv> sellRule)
            : base(outputFunc, context, new[] { buyRule, sellRule })
        {
        }

        public override Func<IEnumerable<IOhlcv>, int, IIndexedOhlcv> IndexedObjectConstructor
            => (l, i) => new IndexedCandle(l, i);
    }
}