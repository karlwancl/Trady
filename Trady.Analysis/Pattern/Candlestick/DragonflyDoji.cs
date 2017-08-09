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
    public class DragonflyDoji<TInput, TOutput> : AnalyzableBase<TInput, (decimal Open, decimal High, decimal Low, decimal Close), bool, TOutput>
    {
        DojiByTuple _doji;

        public DragonflyDoji(IEnumerable<TInput> inputs, Func<TInput, (decimal Open, decimal High, decimal Low, decimal Close)> inputMapper, Func<TInput, bool, TOutput> outputMapper, decimal dojiThreshold = 0.1m, decimal shadowThreshold = 0.1m) : base(inputs, inputMapper, outputMapper)
        {
            _doji = new DojiByTuple(inputs.Select(inputMapper), dojiThreshold);

            DojiThreshold = dojiThreshold;
            ShadowThreshold = shadowThreshold;
        }

        public decimal DojiThreshold { get; }

        public decimal ShadowThreshold { get; }

        protected override bool ComputeByIndexImpl(IEnumerable<(decimal Open, decimal High, decimal Low, decimal Close)> mappedInputs, int index)
        {
			var mean = (Inputs[index].Open + Inputs[index].Close) / 2;
			bool isDragonify = (Inputs[index].High - mean) < ShadowThreshold * (Inputs[index].High - Inputs[index].Low);
			return _doji[index] && isDragonify;        
        }
    }

    public class DragonifyDojiByTuple : DragonflyDoji<(decimal Open, decimal High, decimal Low, decimal Close), bool>
    {
        public DragonifyDojiByTuple(IEnumerable<(decimal Open, decimal High, decimal Low, decimal Close)> inputs, decimal dojiThreshold = 0.1M, decimal shadowThreshold = 0.1M) 
            : base(inputs, i => i, (i, otm) => otm, dojiThreshold, shadowThreshold)
        {
        }
    }

    public class DragonifyDoji : DragonflyDoji<Candle, AnalyzableTick<bool>>
    {
        public DragonifyDoji(IEnumerable<Candle> inputs, , decimal dojiThreshold = 0.1M, decimal shadowThreshold = 0.1M) 
            : base(inputs, i => (i.Open, i.High, i.Low, i.Close), (i, otm) => new AnalyzableTick<bool>(i.DateTime, otm), dojiThreshold, shadowThreshold)
        {
        }
    }
}