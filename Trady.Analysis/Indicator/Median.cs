using System;
using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Infrastructure;
using Trady.Core;
using Trady.Core.Infrastructure;

namespace Trady.Analysis.Indicator
{
    public class Median<TInput, TOutput> : NumericAnalyzableBase<TInput, decimal?, TOutput>
    {
        private PercentileByTuple _percentile;

        public int PeriodCount { get; }

        public Median(IEnumerable<TInput> inputs, Func<TInput, decimal?> inputMapper, int periodCount) : base(inputs, inputMapper)
        {
            _percentile = new PercentileByTuple(inputs.Select(inputMapper), periodCount, 0.5m);
            PeriodCount = periodCount;
        }

        protected override decimal? ComputeByIndexImpl(IReadOnlyList<decimal?> mappedInputs, int index) => _percentile[index];
    }

    public class MedianByTuple : Median<decimal?, decimal?>
    {
        public MedianByTuple(IEnumerable<decimal?> inputs, int periodCount) 
            : base(inputs, i => i, periodCount)
        {
        }

		public MedianByTuple(IEnumerable<decimal> inputs, int periodCount)
	        : this(inputs.Cast<decimal?>(), periodCount)
		{
		}
    }

	public class Median : Median<IOhlcv, AnalyzableTick<decimal?>>
	{
		public Median(IEnumerable<IOhlcv> inputs, int periodCount)
			: base(inputs, i => i.Close, periodCount)
		{
		}
	}
}
