using System;
using System.Collections.Generic;
using Trady.Analysis.Infrastructure;
using Trady.Core;
using Trady.Core.Infrastructure;

namespace Trady.Analysis.Backtest
{
    public class BuySellRuleExecutor : RuleExecutorBase<IOhlcvData, IIndexedOhlcvData, (TransactionType, IIndexedOhlcvData)?>
    {
        public BuySellRuleExecutor(
            Func<IIndexedOhlcvData, int, (TransactionType, IIndexedOhlcvData)?> outputFunc, 
            IAnalyzeContext<IOhlcvData> context, 
            Predicate<IIndexedOhlcvData> buyRule, 
            Predicate<IIndexedOhlcvData> sellRule)
            : base(outputFunc, context, new[] { buyRule, sellRule })
        {
        }

        public override Func<IEnumerable<IOhlcvData>, int, IIndexedOhlcvData> IndexedObjectConstructor
            => (l, i) => new IndexedCandle(l, i);
    }
}