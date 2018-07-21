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
            _rm = new RelativeMomentumByTuple(inputs.Select(inputMapper), rmiPeriod, mtmPeriod);

            MtmPeriod = mtmPeriod;
            RmiPeriod = rmiPeriod;
        }

        protected override decimal? ComputeByIndexImpl(IReadOnlyList<decimal?> mappedInputs, int index)
        {
            var currentRm = _rm[index];
            return currentRm == 0 ? default : 100 * currentRm / (1 + currentRm);
        }    
    }

    public class RelativeMomentumIndexByTuple : RelativeMomentumIndex<decimal?, decimal?>
    {
        public RelativeMomentumIndexByTuple(IEnumerable<decimal?> inputs, int rmiPeriod, int mtmPeriod) 
            : base(inputs, i => i, rmiPeriod, mtmPeriod)
        {
        }
    }

    public class RelativeMomentumIndex : RelativeMomentumIndex<IOhlcv, AnalyzableTick<decimal?>>
    {
        public RelativeMomentumIndex(IEnumerable<IOhlcv> inputs, int rmiPeriod, int mtmPeriod) 
            : base(inputs, i => i.Close, rmiPeriod, mtmPeriod)
        {
        }
    }
}
