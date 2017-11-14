using System;
using System.Collections.Generic;
using Trady.Analysis.Infrastructure;
using System.Linq;
using Trady.Core.Infrastructure;

namespace Trady.Analysis.Candlestick
{
    public class BearishHarami<TInput, TOutput> : AnalyzableBase<TInput, (decimal Open, decimal High, decimal Low, decimal Close), bool?, TOutput>
    {
        private BearishByTuple _bearish;
        private HaramiByTuple _harami;
        private UpTrendByTuple _upTrend;
        private readonly bool _containedShadows;

        public BearishHarami(IEnumerable<TInput> inputs, Func<TInput, (decimal Open, decimal High, decimal Low, decimal Close)> inputMapper, bool containedShadows = false, int uptrendPeriodCount = 3) : base(inputs, inputMapper)
        {
            var mappedInputs = inputs.Select(inputMapper);
            var ocs = mappedInputs.Select(i => (i.Open, i.Close));
            _bearish = new BearishByTuple(ocs);
            _harami = new HaramiByTuple(mappedInputs);
            _upTrend = new UpTrendByTuple(ocs, uptrendPeriodCount);
            _containedShadows = containedShadows;
        }

        protected override bool? ComputeByIndexImpl(IReadOnlyList<(decimal Open, decimal High, decimal Low, decimal Close)> mappedInputs, int index)
        {
            return _harami[index].HasValue && 
                _harami[index].Value && 
                _bearish[index] && 
                _upTrend[index-1].HasValue && 
                _upTrend[index-1].Value;
        }
    }

    public class BearishHaramiByTuple : Harami<(decimal Open, decimal High, decimal Low, decimal Close), bool?>
    {
        public BearishHaramiByTuple(IEnumerable<(decimal Open, decimal High, decimal Low, decimal Close)> inputs, bool containedShadows = false, int uptrendPeriodCount = 3)
            : base(inputs, i => i, containedShadows)
        {
        }
    }

    public class BearishHarami : BearishHarami<IOhlcv, AnalyzableTick<bool?>>
    {
        public BearishHarami(IEnumerable<IOhlcv> inputs, bool containedShadows = false, int uptrendPeriodCount = 3)
            : base(inputs, i => (i.Open, i.High, i.Low, i.Close), containedShadows, uptrendPeriodCount)
        {
        }
    }
}