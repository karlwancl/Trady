using System;
using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Infrastructure;
using Trady.Core.Infrastructure;

namespace Trady.Analysis.Indicator
{
    public class HighestHighLowestLowDifference<TInput, TOutput> : NumericAnalyzableBase<TInput, (decimal High, decimal Low), TOutput>
    {
        private HighestByTuple _hh;
        private LowestByTuple _ll;

        public int PeriodCount { get; }

        public HighestHighLowestLowDifference(IEnumerable<TInput> inputs, Func<TInput, (decimal High, decimal Low)> inputMapper, int periodCount) : base(inputs, inputMapper)
        {
            _hh = new HighestByTuple(inputs.Select(inputMapper).Select(i => i.High), periodCount);
            _ll = new LowestByTuple(inputs.Select(inputMapper).Select(i => i.Low), periodCount);

            PeriodCount = periodCount;
        }

        protected override decimal? ComputeByIndexImpl(IReadOnlyList<(decimal High, decimal Low)> mappedInputs, int index)
            => _hh[index] - _ll[index];
    }

    public class HighestHighLowestLowDifferenceByTuple : HighestHighLowestLowDifference<(decimal High, decimal Low), decimal?>
    {
        public HighestHighLowestLowDifferenceByTuple(IEnumerable<(decimal High, decimal Low)> inputs, int periodCount) 
            : base(inputs, i => i, periodCount)
        {
        }
    }

    public class HighestHighLowestLowDifference : HighestHighLowestLowDifference<IOhlcv, AnalyzableTick<decimal?>>
    {
        public HighestHighLowestLowDifference(IEnumerable<IOhlcv> inputs, int periodCount) 
            : base(inputs, i => (i.High, i.Low), periodCount)
        {
        }
    }
}
