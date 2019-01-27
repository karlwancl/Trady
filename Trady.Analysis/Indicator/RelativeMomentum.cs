using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Trady.Analysis.Infrastructure;
using Trady.Core.Infrastructure;

namespace Trady.Analysis.Indicator
{
    public class RelativeMomentum<TInput, TOutput> : NumericAnalyzableBase<TInput, decimal?, TOutput>
    {
        private readonly PositiveDifferenceByTuple _u;
        private readonly NegativeDifferenceByTuple _d;

        private readonly GenericMovingAverage _dEma;
        private readonly GenericMovingAverage _uEma;

        public RelativeMomentum(IEnumerable<TInput> inputs, Func<TInput, decimal?> inputMapper, int rmiPeriod, int mtmPeriod) : base(inputs, inputMapper)
        {
            _u = new PositiveDifferenceByTuple(inputs.Select(inputMapper), mtmPeriod);
            _d = new NegativeDifferenceByTuple(inputs.Select(inputMapper), mtmPeriod);

            _uEma = new GenericMovingAverage(
                rmiPeriod + mtmPeriod - 1,
                i => Enumerable.Range(i - rmiPeriod + 1, rmiPeriod).Average(j => _u[j]),
                i => _u[i],
                i => 2.0m / (rmiPeriod + 1),
                inputs.Count());

            _dEma = new GenericMovingAverage(
                rmiPeriod + mtmPeriod - 1,
                i => Enumerable.Range(i - rmiPeriod + 1, rmiPeriod).Average(j => _d[j]),
                i => _d[i],
                i => 2.0m / (rmiPeriod + 1),
                inputs.Count());

            MtmPeriod = mtmPeriod;
            RmiPeriod = rmiPeriod;
        }

        public int RmiPeriod { get; }
        public int MtmPeriod { get; }

        protected override decimal? ComputeByIndexImpl(IReadOnlyList<decimal?> mappedInputs, int index)
        {
            var dEma = _dEma[index];
            return dEma.HasValue && dEma != 0 ? _uEma[index] / dEma : default;
        }
    }

    public class RelativeMomentumByTuple : RelativeMomentum<decimal?, decimal?>
    {
        public RelativeMomentumByTuple(IEnumerable<decimal?> inputs, int rmiPeriod, int mtmPeriod)
            : base(inputs, i => i, rmiPeriod, mtmPeriod)
        {
        }
    }

    public class RelativeMomentum : RelativeMomentum<IOhlcv, AnalyzableTick<decimal?>>
    {
        public RelativeMomentum(IEnumerable<IOhlcv> inputs, int rmiPeriod, int mtmPeriod) 
            : base(inputs, i => i.Close, rmiPeriod, mtmPeriod)
        {
        }
    }
}
