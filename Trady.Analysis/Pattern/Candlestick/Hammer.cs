using System;
using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Infrastructure;
using Trady.Core;

namespace Trady.Analysis.Pattern.Candlestick
{
    /// <summary>
    /// Reference: http://www.investopedia.com/terms/h/hammer.asp
    /// </summary>
    public class Hammer<TInput, TOutput> : AnalyzableBase<TInput, (decimal Open, decimal High, decimal Low, decimal Close), bool?, TOutput>
    {
        ShortDayByTuple _shortDay;

        public Hammer(IEnumerable<TInput> inputs, Func<TInput, (decimal Open, decimal High, decimal Low, decimal Close)> inputMapper, Func<TInput, bool?, TOutput> outputMapper, int shortPeriodCount = 20, decimal shortThreshold = 0.25m) : base(inputs, inputMapper, outputMapper)
        {
            var mappedInputs = inputs.Select(inputMapper);
            _shortDay = new ShortDayByTuple(mappedInputs.Select(i => (i.Open, i.Close)), shortPeriodCount, shortThreshold);
        }

        protected override bool? ComputeByIndexImpl(IEnumerable<(decimal Open, decimal High, decimal Low, decimal Close)> mappedInputs, int index)
        {
            throw new NotImplementedException();
        }
    }

    public class HammerByTuple : Hammer<(decimal Open, decimal High, decimal Low, decimal Close), bool?>
    {
        public HammerByTuple(IEnumerable<(decimal Open, decimal High, decimal Low, decimal Close)> inputs, int shortPeriodCount = 20, decimal shortThreshold = 0.25M) 
            : base(inputs, i => i, (i, otm) => otm, shortPeriodCount, shortThreshold)
        {
        }
    }

    public class Hammer : Hammer<Candle, AnalyzableTick<bool?>>
    {
        public Hammer(IEnumerable<Candle> inputs, int shortPeriodCount = 20, decimal shortThreshold = 0.25M) 
            : base(inputs, i => (i.Open, i.High, i.Low, i.Close), (i, otm) => new AnalyzableTick<bool?>(i.DateTime, otm), shortPeriodCount, shortThreshold)
        {
        }
    }
}