using System;
using System.Collections.Generic;
using Trady.Analysis.Infrastructure;
using Trady.Core;
using Trady.Core.Infrastructure;

namespace Trady.Analysis.Backtest
{
    public class BuySellRuleExecutor : RuleExecutorBase<Candle, IndexedCandle, (TransactionType, IndexedCandle)>
    {
        public BuySellRuleExecutor(
            Func<IndexedCandle, int, (TransactionType, IndexedCandle)> outputFunc, 
            IAnalyzeContext<Candle> context, 
            Predicate<IndexedCandle> buyRule, 
            Predicate<IndexedCandle> sellRule)
            : base(outputFunc, context, new Predicate<IndexedCandle>[] { buyRule, sellRule })
        {
        }

        public override Func<IEnumerable<Candle>, int, IndexedCandle> IndexedObjectConstructor
            => (l, i) => new IndexedCandle(l, i);
    }
}