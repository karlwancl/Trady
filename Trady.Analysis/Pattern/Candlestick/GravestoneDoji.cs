using System;
using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Infrastructure;
using Trady.Core;

namespace Trady.Analysis.Pattern.Candlestick
{
    /// <summary>
    /// Reference: http://www.investopedia.com/terms/g/gravestone-doji.asp
    /// </summary>
    public class GravestoneDoji<TInput, TOutput> : AnalyzableBase<TInput, (decimal Open, decimal High, decimal Low, decimal Close), bool, TOutput>
    {
        DojiByTuple _doji;

        public GravestoneDoji(IEnumerable<TInput> inputs, Func<TInput, (decimal Open, decimal High, decimal Low, decimal Close)> inputMapper, decimal dojiThreshold = 0.1m, decimal shadowThreshold = 0.1m) : base(inputs, inputMapper)
        {
            _doji = new DojiByTuple(inputs.Select(inputMapper), dojiThreshold);

            DojiThreshold = dojiThreshold;
            ShadowThreshold = shadowThreshold;
        }

        public decimal DojiThreshold { get; }

        public decimal ShadowThreshold { get; }

        protected override bool ComputeByIndexImpl(IEnumerable<(decimal Open, decimal High, decimal Low, decimal Close)> mappedInputs, int index)
        {
            var mean = (mappedInputs.ElementAt(index).Open + mappedInputs.ElementAt(index).Close) / 2;
            bool isGravestone = (mean - mappedInputs.ElementAt(index).Low) < ShadowThreshold * (mappedInputs.ElementAt(index).High - mappedInputs.ElementAt(index).Low);
			return _doji[index] && isGravestone;
        }
    }

    public class GravestoneDojiByTuple : GravestoneDoji<(decimal Open, decimal High, decimal Low, decimal Close), bool>
    {
        public GravestoneDojiByTuple(IEnumerable<(decimal Open, decimal High, decimal Low, decimal Close)> inputs, decimal dojiThreshold = 0.1M, decimal shadowThreshold = 0.1M) 
            : base(inputs, i => i, dojiThreshold, shadowThreshold)
        {
        }
    }

    public class GravestoneDoji : GravestoneDoji<Candle, AnalyzableTick<bool>>
    {
        public GravestoneDoji(IEnumerable<Candle> inputs, decimal dojiThreshold = 0.1M, decimal shadowThreshold = 0.1M) 
            : base(inputs, i => (i.Open, i.High, i.Low, i.Close), dojiThreshold, shadowThreshold)
        {
        }
    }
}