using System;
using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Infrastructure;
using Trady.Core;

namespace Trady.Analysis.Indicator
{
    public class RawStochasticsValue<TInput, TOutput> : AnalyzableBase<TInput, (decimal High, decimal Low, decimal Close), decimal?, TOutput>
    {
        readonly HighestHighByTuple _hh;
        readonly LowestLowByTuple _ll;

        public RawStochasticsValue(IEnumerable<TInput> inputs, Func<TInput, (decimal High, decimal Low, decimal Close)> inputMapper, int periodCount) : base(inputs, inputMapper)
        {
			_hh = new HighestHighByTuple(inputs.Select(i => inputMapper(i).High), periodCount);
			_ll = new LowestLowByTuple(inputs.Select(i => inputMapper(i).Low), periodCount);

			PeriodCount = periodCount;
        }

        public int PeriodCount { get; }

        protected override decimal? ComputeByIndexImpl(IEnumerable<(decimal High, decimal Low, decimal Close)> mappedInputs, int index)
        {
			var hh = _hh[index];
			var ll = _ll[index];
			return (hh == ll) ? 50 : 100 * (mappedInputs.ElementAt(index).Close - ll) / (hh - ll);        
        }
    }

    public class RawStochasticsValueByTuple : RawStochasticsValue<(decimal High, decimal Low, decimal Close), decimal?>
    {
        public RawStochasticsValueByTuple(IEnumerable<(decimal High, decimal Low, decimal Close)> inputs, int periodCount) 
            : base(inputs, i => i, periodCount)
        {
        }
    }

    public class RawStochasticsValue : RawStochasticsValue<Candle, AnalyzableTick<decimal?>>
    {
        public RawStochasticsValue(IEnumerable<Candle> inputs, int periodCount) 
            : base(inputs, i => (i.High, i.Low, i.Close), periodCount)
        {
        }
    }
}