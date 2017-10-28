using System;
using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Infrastructure;
using Trady.Core;
using Trady.Core.Infrastructure;

namespace Trady.Analysis.Indicator
{
    public class RelativeStrength<TInput, TOutput> : NumericAnalyzableBase<TInput, decimal, TOutput>
    {
        private DifferenceByTuple _closePriceChange;
        private readonly GenericMovingAverage _dEma;
        private readonly GenericMovingAverage _uEma;

        public RelativeStrength(IEnumerable<TInput> inputs, Func<TInput, decimal> inputMapper, int periodCount) : base(inputs, inputMapper)
        {
            _closePriceChange = new DifferenceByTuple(inputs.Select(inputMapper));

            Func<int, decimal?> u = i => i > 0 ? Math.Max(_closePriceChange[i].Value, 0) : (decimal?)null;
            Func<int, decimal?> l = i => i > 0 ? Math.Abs(Math.Min(_closePriceChange[i].Value, 0)) : (decimal?)null;

            _uEma = new GenericMovingAverage(
                periodCount,
                i => i > 0 ? Enumerable.Range(i - PeriodCount + 1, PeriodCount).Average(u) : null,
                u,
                i => 1.0m / periodCount,
                inputs.Count());

            _dEma = new GenericMovingAverage(
                periodCount,
                i => i > 0 ? Enumerable.Range(i - PeriodCount + 1, PeriodCount).Average(l) : null,
                l,
                i => 1.0m / periodCount,
                inputs.Count());

            PeriodCount = periodCount;
        }

        public int PeriodCount { get; }

        protected override decimal? ComputeByIndexImpl(IReadOnlyList<decimal> mappedInputs, int index)
            => _uEma[index] / _dEma[index];
    }

    public class RelativeStrengthByTuple : RelativeStrength<decimal, decimal?>
    {
        public RelativeStrengthByTuple(IEnumerable<decimal> inputs, int periodCount)
            : base(inputs, i => i, periodCount)
        {
        }
    }

    public class RelativeStrength : RelativeStrength<IOhlcvData, AnalyzableTick<decimal?>>
    {
        public RelativeStrength(IEnumerable<IOhlcvData> inputs, int periodCount)
            : base(inputs, i => i.Close, periodCount)
        {
        }
    }
}