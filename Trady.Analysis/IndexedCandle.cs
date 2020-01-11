using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Trady.Core.Infrastructure;

namespace Trady.Analysis
{
    public class IndexedCandle : IndexedCandleBase
    {
        public IndexedCandle(IEnumerable<IOhlcv> candles, int index)
            : base(candles, index)
        {
        }

        protected override IIndexedOhlcv IndexedCandleConstructor(int index)
            => new IndexedCandle(BackingList, index) { Context = Context };
    }
}
