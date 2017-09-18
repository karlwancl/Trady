using System;
using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Infrastructure;
using Trady.Core;

namespace Trady.Analysis.Indicator
{
    public class Median<TInput, TOutput> : NumericAnalyzableBase<TInput, decimal, TOutput>
    {
        private PercentileByTuple _percentile;

        public int PeriodCount { get; }

        public Median(IEnumerable<TInput> inputs, Func<TInput, decimal> inputMapper, int periodCount) : base(inputs, inputMapper)
        {
            PeriodCount = periodCount;
            _percentile = new PercentileByTuple(inputs.Select(inputMapper), periodCount, 0.5m);
        }

        protected override decimal? ComputeByIndexImpl(IReadOnlyList<decimal> mappedInputs, int index) => _percentile[index];
    }

    public class MedianByTuple : Median<decimal, decimal?>
    {
        public MedianByTuple(IEnumerable<decimal> inputs, int periodCount) 
            : base(inputs, i => i, periodCount)
        {
        }
    }

	public class Median : Median<Candle, AnalyzableTick<decimal?>>
	{
		public Median(IEnumerable<Candle> inputs, int periodCount)
			: base(inputs, i => i.Close, periodCount)
		{
		}
	}
}
