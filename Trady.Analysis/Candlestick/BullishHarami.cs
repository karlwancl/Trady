using System;
using System.Collections.Generic;
using Trady.Analysis.Infrastructure;
using System.Linq;
using Trady.Core.Infrastructure;

namespace Trady.Analysis.Candlestick
{
    public class BullishHarami<TInput, TOutput> : AnalyzableBase<TInput, (decimal Open, decimal High, decimal Low, decimal Close), bool?, TOutput>
    {
        private BullishByTuple _bullish;
        private HaramiByTuple _harami;
        private DownTrendByTuple _downTrend;
        private readonly bool _containedShadows;

        public BullishHarami(IEnumerable<TInput> inputs, Func<TInput, (decimal Open, decimal High, decimal Low, decimal Close)> inputMapper, bool containedShadows = false, int downtrendPeriodCount = 3) : base(inputs, inputMapper)
        {
            var mappedInputs = inputs.Select(inputMapper);
            var ocs = mappedInputs.Select(i => (i.Open, i.Close));
            _bullish = new BullishByTuple(ocs);
            _harami = new HaramiByTuple(mappedInputs);
            _downTrend = new DownTrendByTuple(ocs, downtrendPeriodCount);
            _containedShadows = containedShadows;
        }

        protected override bool? ComputeByIndexImpl(IReadOnlyList<(decimal Open, decimal High, decimal Low, decimal Close)> mappedInputs, int index)
        {
            return _harami[index].HasValue &&
                _harami[index].Value &&
                _bullish[index] &&
                _downTrend[index - 1].HasValue &&
                _downTrend[index - 1].Value;
        }
    }

    public class BullishHaramiByTuple : Harami<(decimal Open, decimal High, decimal Low, decimal Close), bool?>
    {
        public BullishHaramiByTuple(IEnumerable<(decimal Open, decimal High, decimal Low, decimal Close)> inputs, bool containedShadows = false, int downTrendPeriodCount = 3)
            : base(inputs, i => i, containedShadows)
        {
        }
    }

    public class BullishHarami : BullishHarami<IOhlcv, AnalyzableTick<bool?>>
    {
        public BullishHarami(IEnumerable<IOhlcv> inputs, bool containedShadows = false, int downTrendPeriodCount = 3)
            : base(inputs, i => (i.Open, i.High, i.Low, i.Close), containedShadows, downTrendPeriodCount)
        {
        }
    }
}