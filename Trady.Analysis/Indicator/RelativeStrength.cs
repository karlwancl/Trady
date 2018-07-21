using System;
using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Infrastructure;
using Trady.Core;
using Trady.Core.Infrastructure;

namespace Trady.Analysis.Indicator
{
    public class RelativeStrength<TInput, TOutput> : NumericAnalyzableBase<TInput, decimal?, TOutput>
    {
        private readonly PositiveDifferenceByTuple _u;
        private readonly NegativeDifferenceByTuple _d;

        private readonly GenericMovingAverage _dEma;
        private readonly GenericMovingAverage _uEma;

        public RelativeStrength(IEnumerable<TInput> inputs, Func<TInput, decimal?> inputMapper, int periodCount) : base(inputs, inputMapper)
        {
            PeriodCount = periodCount;

            _u = new PositiveDifferenceByTuple(inputs.Select(inputMapper), 1);
            _d = new NegativeDifferenceByTuple(inputs.Select(inputMapper), 1);

            _uEma = new GenericMovingAverage(
                periodCount,
                i => Enumerable.Range(i - PeriodCount + 1, PeriodCount).Average(j => _u[j]),
                i => _u[i],
                Smoothing.Mma(periodCount),
                inputs.Count());

            _dEma = new GenericMovingAverage(
                periodCount,
                i => Enumerable.Range(i - PeriodCount + 1, PeriodCount).Average(j => _d[j]),
                i => _d[i],
                Smoothing.Mma(periodCount),
                inputs.Count());
        }

        public int PeriodCount { get; }

        protected override decimal? ComputeByIndexImpl(IReadOnlyList<decimal?> mappedInputs, int index)
        {
            var dEma = _dEma[index];
            return dEma.HasValue && dEma != 0 ? _uEma[index] / dEma : default;
        }
    }

    public class RelativeStrengthByTuple : RelativeStrength<decimal?, decimal?>
    {
        public RelativeStrengthByTuple(IEnumerable<decimal?> inputs, int periodCount)
            : base(inputs, i => i, periodCount)
        {
        }
    }

    public class RelativeStrength : RelativeStrength<IOhlcv, AnalyzableTick<decimal?>>
    {
        public RelativeStrength(IEnumerable<IOhlcv> inputs, int periodCount)
            : base(inputs, i => i.Close, periodCount)
        {
        }
    }
}
