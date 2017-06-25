using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Trady.Analysis.Infrastructure;
using Trady.Core;

namespace Trady.Analysis.Pattern.Candlestick
{
    public class Bullish : AnalyzableBase<(decimal Open, decimal Close), bool>
    {
        public Bullish(IList<Candle> candles)
            : this (candles.Select(c => (c.Open, c.Close)).ToList())
        {
        }

        public Bullish(IList<(decimal Open, decimal Close)> inputs) : base(inputs)
        {
        }

        protected override bool ComputeByIndexImpl(int index)
            => Inputs[index].Open < Inputs[index].Close;
    }
}
