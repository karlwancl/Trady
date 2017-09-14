using System;
using System.Collections.Generic;
using Trady.Analysis.Strategy.Rule;
using Trady.Core;

namespace Trady.Analysis.Strategy.Portfolio
{
    public class BuySellRuleExecutor : RuleExecutorBase<Candle, IndexedCandle, (TransactionType, IndexedCandle)>
    {
        public BuySellRuleExecutor(Func<IndexedCandle, int, (TransactionType, IndexedCandle)> outputFunc, Func<IRule<IndexedCandle>> buyRule, Func<IRule<IndexedCandle>> sellRule)
            : base(outputFunc, buyRule, sellRule)
        {
        }

        public override Func<IEnumerable<Candle>, int, IndexedCandle> IndexedObjectConstructor
            => (l, i) => new IndexedCandle(l, i);
    }
}