using System;
using System.Collections.Generic;
using Trady.Core;
using Trady.Core.Infrastructure;

namespace Trady.Analysis.Indicator
{
	public class Momentum : Difference<IOhlcv, AnalyzableTick<decimal?>>
	{
        public Momentum(IEnumerable<IOhlcv> inputs, int periodCount = 1)
			: base(inputs, i => i.Close, periodCount)
		{
		}
	}
}
