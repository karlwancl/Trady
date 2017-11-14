using System;
using System.Collections.Generic;
using System.Linq;

using Trady.Analysis.Infrastructure;
using Trady.Core.Infrastructure;

namespace Trady.Analysis.Candlestick
{
    /// <summary>
    /// Reference: http://www.investopedia.com/terms/h/hammer.asp
    /// </summary>
    public class Hammer<TInput, TOutput> : AnalyzableBase<TInput, (decimal Open, decimal High, decimal Low, decimal Close), bool?, TOutput>
    {
        private ShortDayByTuple _shortDay;

        public Hammer(IEnumerable<TInput> inputs, Func<TInput, (decimal Open, decimal High, decimal Low, decimal Close)> inputMapper, int shortPeriodCount = 20, decimal shortThreshold = 0.25m) : base(inputs, inputMapper)
        {
            var mappedInputs = inputs.Select(inputMapper);
            _shortDay = new ShortDayByTuple(mappedInputs.Select(i => (i.Open, i.Close)), shortPeriodCount, shortThreshold);
        }

        protected override bool? ComputeByIndexImpl(IReadOnlyList<(decimal Open, decimal High, decimal Low, decimal Close)> mappedInputs, int index)
        {
            throw new NotImplementedException();
        }
    }

    public class HammerByTuple : Hammer<(decimal Open, decimal High, decimal Low, decimal Close), bool?>
    {
        public HammerByTuple(IEnumerable<(decimal Open, decimal High, decimal Low, decimal Close)> inputs, int shortPeriodCount = 20, decimal shortThreshold = 0.25M)
            : base(inputs, i => i, shortPeriodCount, shortThreshold)
        {
        }
    }

    public class Hammer : Hammer<IOhlcv, AnalyzableTick<bool?>>
    {
        public Hammer(IEnumerable<IOhlcv> inputs, int shortPeriodCount = 20, decimal shortThreshold = 0.25M)
            : base(inputs, i => (i.Open, i.High, i.Low, i.Close), shortPeriodCount, shortThreshold)
        {
        }
    }
}