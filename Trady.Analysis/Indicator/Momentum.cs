using System;
using System.Collections.Generic;
using Trady.Core;

namespace Trady.Analysis.Indicator
{
	public class Momentum : Difference<Candle, AnalyzableTick<decimal?>>
	{
        public Momentum(IEnumerable<Candle> inputs, int numberOfDays = 1)
			: base(inputs, i => i.Close, numberOfDays)
		{
		}
	}
}
