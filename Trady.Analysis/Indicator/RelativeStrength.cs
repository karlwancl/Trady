using System;
using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Infrastructure;
using Trady.Core;

namespace Trady.Analysis.Indicator
{
    public class RelativeStrength<TInput, TOutput> : AnalyzableBase<TInput, decimal, decimal?, TOutput>
    {
        ClosePriceChangeByTuple _closePriceChange;
        GenericExponentialMovingAverage _uEma, _dEma;

        public RelativeStrength(IEnumerable<TInput> inputs, Func<TInput, decimal> inputMapper, int periodCount) : base(inputs, inputMapper)
        {
            _closePriceChange = new ClosePriceChangeByTuple(inputs.Select(inputMapper));

            Func<int, decimal?> u = i => i > 0 ? Math.Max(_closePriceChange[i].Value, 0) : (decimal?)null;
            Func<int, decimal?> l = i => i > 0 ? Math.Abs(Math.Min(_closePriceChange[i].Value, 0)) : (decimal?)null;

            _uEma = new GenericExponentialMovingAverage(
                periodCount,
                i => i > 0 ? Enumerable.Range(i - PeriodCount + 1, PeriodCount).Average(j => u(j)) : null,
                i => u(i),
                i => 1.0m / periodCount,
                inputs.Count());

            _dEma = new GenericExponentialMovingAverage(
                periodCount,
                i => i > 0 ? Enumerable.Range(i - PeriodCount + 1, PeriodCount).Average(j => l(j)) : null,
                i => l(i),
                i => 1.0m / periodCount,
                inputs.Count());

            PeriodCount = periodCount;
        }

        public int PeriodCount { get; }

        protected override decimal? ComputeByIndexImpl(IEnumerable<decimal> mappedInputs, int index)
        {
            var uEma = _uEma[index];
            var dEma = _dEma[index];
            var result = uEma / dEma;
            return result;
        }
    }

    public class RelativeStrengthByTuple : RelativeStrength<decimal, decimal?>
    {
        public RelativeStrengthByTuple(IEnumerable<decimal> inputs, int periodCount) 
            : base(inputs, i => i, periodCount)
        {
        }
    }

    public class RelativeStrength : RelativeStrength<Candle, AnalyzableTick<decimal?>>
    {
        public RelativeStrength(IEnumerable<Candle> inputs, int periodCount) 
            : base(inputs, i => i.Close, periodCount)
        {
        }
    }
}