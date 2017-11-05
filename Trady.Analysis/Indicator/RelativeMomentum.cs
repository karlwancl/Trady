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
            MtmPeriod = mtmPeriod;
            RmiPeriod = rmiPeriod;

            _u = new PositiveDifferenceByTuple(inputs.Select(inputMapper), MtmPeriod);
            _d = new NegativeDifferenceByTuple(inputs.Select(inputMapper), MtmPeriod);

            _uEma = new GenericMovingAverage(
                RmiPeriod + MtmPeriod - 1,
                i => Enumerable.Range(i - RmiPeriod + 1, RmiPeriod).Average(j => _u[j]),
                i => _u[i],
                i => 2.0m / (RmiPeriod + 1),
                inputs.Count());

            _dEma = new GenericMovingAverage(
                RmiPeriod + MtmPeriod - 1,
                i => Enumerable.Range(i - RmiPeriod + 1, RmiPeriod).Average(j => _d[j]),
                i => _d[i],
                i => 2.0m / (RmiPeriod + 1),
                inputs.Count());
        }

        public int RmiPeriod { get; }
        public int MtmPeriod { get; }

        protected override decimal? ComputeByIndexImpl(IReadOnlyList<decimal?> mappedInputs, int index)
        {
            var dEma = _dEma[index];
            return dEma.HasValue && dEma != 0 ? _uEma[index] / dEma : default(decimal?);
        }
    }

    public class RelativeMomentumByTuple : RelativeMomentum<decimal?, decimal?>
    {
        public RelativeMomentumByTuple(IEnumerable<decimal?> inputs, int rmiPeriod, int mtmPeriod)
            : base(inputs, i => i, rmiPeriod, mtmPeriod)
        {
        }
    }

    public class RelativeMomentum : RelativeMomentum<IOhlcvData, AnalyzableTick<decimal?>>
    {
        public RelativeMomentum(IEnumerable<IOhlcvData> inputs, int rmiPeriod, int mtmPeriod) 
            : base(inputs, i => i.Close, rmiPeriod, mtmPeriod)
        {
        }
    }
}
