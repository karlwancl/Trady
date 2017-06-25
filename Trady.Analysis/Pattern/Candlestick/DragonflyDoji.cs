using System;
using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Infrastructure;
using Trady.Core;

namespace Trady.Analysis.Pattern.Candlestick
{
    /// <summary>
    /// Reference: http://www.investopedia.com/terms/d/dragonfly-doji.asp
    /// </summary>
    public class DragonflyDoji : AnalyzableBase<(decimal Open, decimal High, decimal Low, decimal Close), bool>
    {
        private Doji _doji;

        public DragonflyDoji(IList<Candle> candles, decimal dojiThreshold = 0.1m, decimal shadowThreshold = 0.1m)
            : this(candles.Select(c => (c.Open, c.High, c.Low, c.Close)).ToList(), dojiThreshold, shadowThreshold)
        {
        }

        public DragonflyDoji(IList<(decimal Open, decimal High, decimal Low, decimal Close)> inputs, decimal dojiThreshold = 0.1m, decimal shadowThreshold = 0.1m) : base(inputs)
        {
            _doji = new Doji(inputs, dojiThreshold);

            DojiThreshold = dojiThreshold;
            ShadowThreshold = shadowThreshold;
        }

        public decimal DojiThreshold { get; private set; }

        public decimal ShadowThreshold { get; private set; }

        protected override bool ComputeByIndexImpl(int index)
        {
            var mean = (Inputs[index].Open + Inputs[index].Close) / 2;
            bool isDragonify = (Inputs[index].High - mean) < ShadowThreshold * (Inputs[index].High - Inputs[index].Low);
            return _doji[index] && isDragonify;
        }
    }
}