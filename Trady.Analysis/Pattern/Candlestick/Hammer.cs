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
    public class Hammer : AnalyzableBase<(decimal Open, decimal High, decimal Low, decimal Close), bool?>
    {
        private ShortDay _shortDay;

        public Hammer(IList<Candle> candles, int shortPeriodCount = 20, decimal shortThreshold = 0.25m)
            : this (candles.Select(c => (c.Open, c.High, c.Low, c.Close)).ToList(), shortPeriodCount, shortThreshold)
        {
        }

        public Hammer(IList<(decimal Open, decimal High, decimal Low, decimal Close)> inputs, int shortPeriodCount = 20, decimal shortThreshold = 0.25m) : base(inputs)
        {
            _shortDay = new ShortDay(inputs.Select(i => (i.Open, i.Close)).ToList(), shortPeriodCount, shortThreshold);
        }

        protected override bool? ComputeByIndexImpl(int index)
        {
            throw new NotImplementedException();
        }
    }
}