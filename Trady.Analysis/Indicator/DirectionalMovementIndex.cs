using System;
using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Infrastructure;
using Trady.Core;
using Trady.Core.Infrastructure;

namespace Trady.Analysis.Indicator
{
    public class DirectionalMovementIndex<TInput, TOutput> : NumericAnalyzableBase<TInput, (decimal High, decimal Low, decimal Close), TOutput>
    {
        private readonly PlusDirectionalIndicatorByTuple _pdi;
        private readonly MinusDirectionalIndicatorByTuple _mdi;

        public DirectionalMovementIndex(IEnumerable<TInput> inputs, Func<TInput, (decimal High, decimal Low, decimal Close)> inputMapper, int periodCount)
            : base(inputs, inputMapper)
        {
            _pdi = new PlusDirectionalIndicatorByTuple(inputs.Select(inputMapper), periodCount);
            _mdi = new MinusDirectionalIndicatorByTuple(inputs.Select(inputMapper), periodCount);
            PeriodCount = periodCount;
        }

        public int PeriodCount { get; }

        protected override decimal? ComputeByIndexImpl(IReadOnlyList<(decimal High, decimal Low, decimal Close)> mappedInputs, int index)
        {
            var value = (_pdi[index] - _mdi[index]) / (_pdi[index] + _mdi[index]);
            return value.HasValue ? Math.Abs(value.Value) * 100 : (decimal?)null;
        }
    }

    public class DirectionalMovementIndexByTuple : DirectionalMovementIndex<(decimal High, decimal Low, decimal Close), decimal?>
    {
        public DirectionalMovementIndexByTuple(IEnumerable<(decimal High, decimal Low, decimal Close)> inputs, int periodCount)
            : base(inputs, i => i, periodCount)
        {
        }
    }

    public class DirectionalMovementIndex : DirectionalMovementIndex<IOhlcv, AnalyzableTick<decimal?>>
    {
        public DirectionalMovementIndex(IEnumerable<IOhlcv> inputs, int periodCount)
            : base(inputs, i => (i.High, i.Low, i.Close), periodCount)
        {
        }
    }
}