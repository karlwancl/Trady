using System;
using System.Collections.Generic;
using System.Linq;

using Trady.Analysis.Infrastructure;
using Trady.Core.Infrastructure;

namespace Trady.Analysis.Candlestick
{
    /// <summary>
    /// Reference: http://www.investopedia.com/terms/d/dragonfly-doji.asp
    /// </summary>
    /// <typeparam name="TInput"></typeparam>
    /// <typeparam name="TOutput"></typeparam>
    public class DragonflyDoji<TInput, TOutput> : AnalyzableBase<TInput, (decimal Open, decimal High, decimal Low, decimal Close), bool, TOutput>
    {
        private DojiByTuple _doji;

        public DragonflyDoji(IEnumerable<TInput> inputs, Func<TInput, (decimal Open, decimal High, decimal Low, decimal Close)> inputMapper, decimal dojiThreshold = 0.1m, decimal shadowThreshold = 0.1m) : base(inputs, inputMapper)
        {
            _doji = new DojiByTuple(inputs.Select(inputMapper), dojiThreshold);

            DojiThreshold = dojiThreshold;
            ShadowThreshold = shadowThreshold;
        }

        public decimal DojiThreshold { get; }

        public decimal ShadowThreshold { get; }

        protected override bool ComputeByIndexImpl(IReadOnlyList<(decimal Open, decimal High, decimal Low, decimal Close)> mappedInputs, int index)
        {
            var mean = (mappedInputs[index].Open + mappedInputs[index].Close) / 2;
            var isDragonify = (mappedInputs[index].High - mean) < ShadowThreshold * (mappedInputs[index].High - mappedInputs[index].Low);
            return _doji[index] && isDragonify;
        }
    }

    public class DragonifyDojiByTuple : DragonflyDoji<(decimal Open, decimal High, decimal Low, decimal Close), bool>
    {
        public DragonifyDojiByTuple(IEnumerable<(decimal Open, decimal High, decimal Low, decimal Close)> inputs, decimal dojiThreshold = 0.1M, decimal shadowThreshold = 0.1M)
            : base(inputs, i => i, dojiThreshold, shadowThreshold)
        {
        }
    }

    public class DragonifyDoji : DragonflyDoji<IOhlcv, AnalyzableTick<bool>>
    {
        public DragonifyDoji(IEnumerable<IOhlcv> inputs, decimal dojiThreshold = 0.1M, decimal shadowThreshold = 0.1M)
            : base(inputs, i => (i.Open, i.High, i.Low, i.Close), dojiThreshold, shadowThreshold)
        {
        }
    }
}
