using System;
using System.Collections.Generic;
using Trady.Core;
using Trady.Core.Infrastructure;

namespace Trady.Analysis.Indicator
{
	public class Momentum : Difference<IOhlcvData, AnalyzableTick<decimal?>>
	{
        public Momentum(IEnumerable<IOhlcvData> inputs, int numberOfDays = 1)
			: base(inputs, i => i.Close, numberOfDays)
		{
		}
	}
}
