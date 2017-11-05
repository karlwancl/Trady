using System;
using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Infrastructure;
using Trady.Core.Infrastructure;

namespace Trady.Analysis.Indicator
{
    public class RelativeMomentumIndex<TInput, TOutput> : NumericAnalyzableBase<TInput, decimal?, TOutput>
    {
        public int RmiPeriod { get; }
        public int MtmPeriod { get; }

        private readonly RelativeMomentumByTuple _rm;

        public RelativeMomentumIndex(IEnumerable<TInput> inputs, Func<TInput, decimal?> inputMapper, int rmiPeriod, int mtmPeriod) : base(inputs, inputMapper)
        {
            MtmPeriod = mtmPeriod;
            RmiPeriod = rmiPeriod;

            _rm = new RelativeMomentumByTuple(inputs.Select(inputMapper), rmiPeriod, mtmPeriod);
        }

        protected override decimal? ComputeByIndexImpl(IReadOnlyList<decimal?> mappedInputs, int index)
            => 100 * _rm[index] / (1 + _rm[index]);
    }

    public class RelativeMomentumIndexByTuple : RelativeMomentumIndex<decimal?, decimal?>
    {
        public RelativeMomentumIndexByTuple(IEnumerable<decimal?> inputs, int rmiPeriod, int mtmPeriod) 
            : base(inputs, i => i, rmiPeriod, mtmPeriod)
        {
        }
    }

    public class RelativeMomentumIndex : RelativeMomentumIndex<IOhlcvData, AnalyzableTick<decimal?>>
    {
        public RelativeMomentumIndex(IEnumerable<IOhlcvData> inputs, int rmiPeriod, int mtmPeriod) 
            : base(inputs, i => i.Close, rmiPeriod, mtmPeriod)
        {
        }
    }
}
