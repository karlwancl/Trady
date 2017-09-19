using System;
using System.Collections.Generic;
using Trady.Core;

namespace Trady.Analysis.Indicator
{
	public class ClosePriceChange : Difference<Candle, AnalyzableTick<decimal?>>
	{
        public ClosePriceChange(IEnumerable<Candle> inputs, int numberOfDays = 1)
			: base(inputs, i => i.Close, numberOfDays)
		{
		}
	}
}
