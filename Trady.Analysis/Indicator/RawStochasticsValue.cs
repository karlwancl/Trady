using System;
using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Infrastructure;
using Trady.Core;
using Trady.Core.Infrastructure;

namespace Trady.Analysis.Indicator
{
    public class RawStochasticsValue<TInput, TOutput> : NumericAnalyzableBase<TInput, (decimal High, decimal Low, decimal Close), TOutput>
    {
        private readonly HighestByTuple _hh;
        private readonly LowestByTuple _ll;

        public RawStochasticsValue(IEnumerable<TInput> inputs, Func<TInput, (decimal High, decimal Low, decimal Close)> inputMapper, int periodCount) : base(inputs, inputMapper)
        {
            _hh = new HighestByTuple(inputs.Select(i => inputMapper(i).High), periodCount);
            _ll = new LowestByTuple(inputs.Select(i => inputMapper(i).Low), periodCount);

            PeriodCount = periodCount;
        }

        public int PeriodCount { get; }

        protected override decimal? ComputeByIndexImpl(IReadOnlyList<(decimal High, decimal Low, decimal Close)> mappedInputs, int index)
        {
            var hh = _hh[index];
            var ll = _ll[index];
            return (hh == ll) ? 50 : 100 * (mappedInputs[index].Close - ll) / (hh - ll);
        }
    }

    public class RawStochasticsValueByTuple : RawStochasticsValue<(decimal High, decimal Low, decimal Close), decimal?>
    {
        public RawStochasticsValueByTuple(IEnumerable<(decimal High, decimal Low, decimal Close)> inputs, int periodCount)
            : base(inputs, i => i, periodCount)
        {
        }
    }

    public class RawStochasticsValue : RawStochasticsValue<IOhlcv, AnalyzableTick<decimal?>>
    {
        public RawStochasticsValue(IEnumerable<IOhlcv> inputs, int periodCount)
            : base(inputs, i => (i.High, i.Low, i.Close), periodCount)
        {
        }
    }
}